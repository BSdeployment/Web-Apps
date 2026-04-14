using Backend.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projects;

    public ProjectsController(IProjectService projects)
    {
        _projects = projects;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll(CancellationToken ct)
    {
        var items = await _projects.GetAllAsync(ct);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(id, ct);
        if (project == null)
        {
            return NotFound(ApiErrors.NotFound($"Project {id} was not found."));
        }

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(ProjectCreateDto dto, CancellationToken ct)
    {
        var created = await _projects.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProjectDto>> Update(int id, ProjectUpdateDto dto, CancellationToken ct)
    {
        var updated = await _projects.UpdateAsync(id, dto, ct);
        if (updated == null)
        {
            return NotFound(ApiErrors.NotFound($"Project {id} was not found."));
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _projects.DeleteAsync(id, ct);
        if (!deleted)
        {
            return NotFound(ApiErrors.NotFound($"Project {id} was not found."));
        }

        return NoContent();
    }
}
