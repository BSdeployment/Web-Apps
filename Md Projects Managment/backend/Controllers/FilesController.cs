using Backend.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api")]
public class FilesController : ControllerBase
{
    private readonly IFileService _files;

    public FilesController(IFileService files)
    {
        _files = files;
    }

    [HttpGet("projects/{projectId:int}/files")]
    public async Task<ActionResult<List<ProjectFileDto>>> GetByProject(int projectId, CancellationToken ct)
    {
        var items = await _files.GetByProjectIdAsync(projectId, ct);
        return Ok(items);
    }

    [HttpPost("projects/{projectId:int}/files")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ProjectFileDto>> Upload(int projectId, [FromForm] FileUploadRequest request, CancellationToken ct)
    {
        if (request.File == null)
        {
            return BadRequest(ApiErrors.BadRequest("File is required."));
        }

        var created = await _files.UploadAsync(projectId, request.File, ct);
        if (created == null)
        {
            return NotFound(ApiErrors.NotFound($"Project {projectId} was not found or file was empty."));
        }

        return CreatedAtAction(nameof(GetByProject), new { projectId = created.ProjectId }, created);
    }

    [HttpGet("files/{id:int}/download")]
    public async Task<IActionResult> Download(int id, CancellationToken ct)
    {
        var result = await _files.OpenFileAsync(id, ct);
        if (result == null)
        {
            return NotFound(ApiErrors.NotFound($"File {id} was not found."));
        }

        var (file, stream) = result.Value;
        var contentType = string.IsNullOrWhiteSpace(file.FileType) ? "application/octet-stream" : file.FileType;
        return File(stream, contentType, file.FileName);
    }

    [HttpGet("projects/{projectId:int}/files/zip")]
    public async Task<IActionResult> DownloadZip(int projectId, CancellationToken ct)
    {
        var result = await _files.CreateProjectZipAsync(projectId, ct);
        if (result == null)
        {
            return NotFound(ApiErrors.NotFound($"Project {projectId} was not found."));
        }

        var (fileName, stream) = result.Value;
        return File(stream, "application/zip", fileName);
    }

    [HttpDelete("files/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _files.DeleteAsync(id, ct);
        if (!deleted)
        {
            return NotFound(ApiErrors.NotFound($"File {id} was not found."));
        }

        return NoContent();
    }

    public class FileUploadRequest
    {
        public IFormFile File { get; set; }
    }
}
