using Backend.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/projects/{projectId:int}/notes")]
public class NotesController : ControllerBase
{
    private readonly INoteService _notes;

    public NotesController(INoteService notes)
    {
        _notes = notes;
    }

    [HttpGet]
    public async Task<ActionResult<ProjectNoteDto>> Get(int projectId, CancellationToken ct)
    {
        var note = await _notes.GetByProjectIdAsync(projectId, ct);
        if (note == null)
        {
            return NotFound(ApiErrors.NotFound($"Note for project {projectId} was not found."));
        }

        return Ok(note);
    }

    [HttpPut]
    public async Task<IActionResult> Upsert(int projectId, ProjectNoteUpdateDto dto, CancellationToken ct)
    {
        var result = await _notes.UpsertAsync(projectId, dto, ct);
        return result.Result switch
        {
            NoteSaveResultType.NotFound => NotFound(ApiErrors.NotFound($"Project {projectId} was not found.")),
            NoteSaveResultType.Deleted => NoContent(),
            _ => Ok(result.Note)
        };
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int projectId, CancellationToken ct)
    {
        var deleted = await _notes.DeleteAsync(projectId, ct);
        if (!deleted)
        {
            return NotFound(ApiErrors.NotFound($"Note for project {projectId} was not found."));
        }

        return NoContent();
    }
}
