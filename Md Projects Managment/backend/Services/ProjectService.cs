using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync(CancellationToken ct);
    Task<ProjectDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<ProjectDto> CreateAsync(ProjectCreateDto dto, CancellationToken ct);
    Task<ProjectDto?> UpdateAsync(int id, ProjectUpdateDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projects;

    public ProjectService(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<List<ProjectDto>> GetAllAsync(CancellationToken ct)
    {
        var items = await _projects.GetAllAsync(ct);
        return items.Select(Map).ToList();
    }

    public async Task<ProjectDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(id, ct);
        return project == null ? null : Map(project);
    }

    public async Task<ProjectDto> CreateAsync(ProjectCreateDto dto, CancellationToken ct)
    {
        var project = new Project
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim(),
            Completed = dto.Completed,
            CreatedAt = DateTime.UtcNow
        };

        await _projects.AddAsync(project, ct);
        return Map(project);
    }

    public async Task<ProjectDto?> UpdateAsync(int id, ProjectUpdateDto dto, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(id, ct);
        if (project == null)
        {
            return null;
        }

        project.Title = dto.Title.Trim();
        project.Description = dto.Description?.Trim();
        project.Completed = dto.Completed;

        await _projects.UpdateAsync(project, ct);
        return Map(project);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(id, ct);
        if (project == null)
        {
            return false;
        }

        await _projects.DeleteAsync(project, ct);
        return true;
    }

    private static ProjectDto Map(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            Completed = project.Completed
        };
    }
}
