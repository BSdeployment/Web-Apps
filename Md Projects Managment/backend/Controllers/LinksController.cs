using Backend.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api")]
public class LinksController : ControllerBase
{
    private readonly ILinkService _links;

    public LinksController(ILinkService links)
    {
        _links = links;
    }

    [HttpGet("projects/{projectId:int}/links")]
    public async Task<ActionResult<List<ProjectLinkDto>>> GetByProject(int projectId, CancellationToken ct)
    {
        var items = await _links.GetByProjectIdAsync(projectId, ct);
        return Ok(items);
    }

    [HttpPost("projects/{projectId:int}/links")]
    public async Task<ActionResult<ProjectLinkDto>> Create(int projectId, ProjectLinkCreateDto dto, CancellationToken ct)
    {
        var created = await _links.CreateAsync(projectId, dto, ct);
        if (created == null)
        {
            return NotFound(ApiErrors.NotFound($"Project {projectId} was not found."));
        }

        return CreatedAtAction(nameof(GetByProject), new { projectId = created.ProjectId }, created);
    }

    [HttpDelete("links/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _links.DeleteAsync(id, ct);
        if (!deleted)
        {
            return NotFound(ApiErrors.NotFound($"Link {id} was not found."));
        }

        return NoContent();
    }
}
