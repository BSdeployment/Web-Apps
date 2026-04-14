using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public interface IFileRepository
{
    Task<List<ProjectFile>> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<ProjectFile?> GetByIdAsync(int id, CancellationToken ct);
    Task<ProjectFile> AddAsync(ProjectFile file, CancellationToken ct);
    Task DeleteAsync(ProjectFile file, CancellationToken ct);
}

public class FileRepository : IFileRepository
{
    private readonly WritingDbContext _db;

    public FileRepository(WritingDbContext db)
    {
        _db = db;
    }

    public Task<List<ProjectFile>> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        return _db.ProjectFiles.AsNoTracking().Where(f => f.ProjectId == projectId).ToListAsync(ct);
    }

    public Task<ProjectFile?> GetByIdAsync(int id, CancellationToken ct)
    {
        return _db.ProjectFiles.FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<ProjectFile> AddAsync(ProjectFile file, CancellationToken ct)
    {
        _db.ProjectFiles.Add(file);
        await _db.SaveChangesAsync(ct);
        return file;
    }

    public async Task DeleteAsync(ProjectFile file, CancellationToken ct)
    {
        _db.ProjectFiles.Remove(file);
        await _db.SaveChangesAsync(ct);
    }
}
