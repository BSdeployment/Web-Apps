using System.IO.Compression;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.Extensions.Options;

namespace Backend.Services;

public interface IFileService
{
    Task<List<ProjectFileDto>> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<ProjectFileDto?> UploadAsync(int projectId, IFormFile file, CancellationToken ct);
    Task<(ProjectFile File, Stream Stream)?> OpenFileAsync(int id, CancellationToken ct);
    Task<(string FileName, Stream Stream)?> CreateProjectZipAsync(int projectId, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public class FileService : IFileService
{
    private readonly IFileRepository _files;
    private readonly IProjectRepository _projects;
    private readonly StorageSettings _storage;
    private readonly ILogger<FileService> _logger;

    public FileService(IFileRepository files, IProjectRepository projects, IOptions<StorageSettings> storage, ILogger<FileService> logger)
    {
        _files = files;
        _projects = projects;
        _storage = storage.Value;
        _logger = logger;
    }

    public async Task<List<ProjectFileDto>> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        var items = await _files.GetByProjectIdAsync(projectId, ct);
        return items.Select(Map).ToList();
    }

    public async Task<ProjectFileDto?> UploadAsync(int projectId, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0)
        {
            return null;
        }

        var project = await _projects.GetByIdAsync(projectId, ct);
        if (project == null)
        {
            return null;
        }

        var safeFileName = Path.GetFileName(file.FileName);
        var storageRoot = Path.GetFullPath(_storage.RootPath);
        var projectFolder = Path.Combine(storageRoot, $"project_{projectId}");
        Directory.CreateDirectory(projectFolder);

        var storedName = $"{Guid.NewGuid():N}_{safeFileName}";
        var fullPath = Path.Combine(projectFolder, storedName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, ct);
        }

        var relativePath = Path.GetRelativePath(storageRoot, fullPath);

        var record = new ProjectFile
        {
            ProjectId = projectId,
            FileName = safeFileName,
            FilePath = relativePath.Replace("\\", "/"),
            FileType = file.ContentType
        };

        await _files.AddAsync(record, ct);
        _logger.LogInformation("Stored file {FileName} for project {ProjectId}", safeFileName, projectId);
        return Map(record);
    }

    public async Task<(ProjectFile File, Stream Stream)?> OpenFileAsync(int id, CancellationToken ct)
    {
        var file = await _files.GetByIdAsync(id, ct);
        if (file == null)
        {
            return null;
        }

        var storageRoot = Path.GetFullPath(_storage.RootPath);
        var fullPath = Path.Combine(storageRoot, file.FilePath.Replace("/", "\\"));
        if (!File.Exists(fullPath))
        {
            return null;
        }

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return (file, stream);
    }

    public async Task<(string FileName, Stream Stream)?> CreateProjectZipAsync(int projectId, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(projectId, ct);
        if (project == null)
        {
            return null;
        }

        var files = await _files.GetByProjectIdAsync(projectId, ct);
        var storageRoot = Path.GetFullPath(_storage.RootPath);

        var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var file in files)
            {
                var fullPath = Path.Combine(storageRoot, file.FilePath.Replace("/", "\\"));
                if (!File.Exists(fullPath))
                {
                    continue;
                }

                var entryName = $"{file.Id}_{file.FileName}";
                var entry = archive.CreateEntry(entryName, CompressionLevel.Fastest);
                await using var entryStream = entry.Open();
                await using var sourceStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                await sourceStream.CopyToAsync(entryStream, ct);
            }
        }

        zipStream.Position = 0;
        var zipFileName = $"project_{projectId}_files.zip";
        return (zipFileName, zipStream);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var file = await _files.GetByIdAsync(id, ct);
        if (file == null)
        {
            return false;
        }

        var storageRoot = Path.GetFullPath(_storage.RootPath);
        var fullPath = Path.Combine(storageRoot, file.FilePath.Replace("/", "\\"));
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete file on disk: {FilePath}", fullPath);
        }

        await _files.DeleteAsync(file, ct);
        return true;
    }

    private static ProjectFileDto Map(ProjectFile file)
    {
        return new ProjectFileDto
        {
            Id = file.Id,
            ProjectId = file.ProjectId,
            FileName = file.FileName,
            FilePath = file.FilePath,
            FileType = file.FileType
        };
    }
}
