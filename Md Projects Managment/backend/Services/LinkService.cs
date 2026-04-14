using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public interface ILinkService
{
    Task<List<ProjectLinkDto>> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<ProjectLinkDto?> CreateAsync(int projectId, ProjectLinkCreateDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public class LinkService : ILinkService
{
    private readonly ILinkRepository _links;
    private readonly IProjectRepository _projects;

    public LinkService(ILinkRepository links, IProjectRepository projects)
    {
        _links = links;
        _projects = projects;
    }

    public async Task<List<ProjectLinkDto>> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        var items = await _links.GetByProjectIdAsync(projectId, ct);
        return items.Select(Map).ToList();
    }

    public async Task<ProjectLinkDto?> CreateAsync(int projectId, ProjectLinkCreateDto dto, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(projectId, ct);
        if (project == null)
        {
            return null;
        }

        var link = new ProjectLink
        {
            ProjectId = projectId,
            Url = dto.Url.Trim(),
            Title = dto.Title?.Trim(),
            Notes = dto.Notes?.Trim()
        };

        await _links.AddAsync(link, ct);
        return Map(link);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var link = await _links.GetByIdAsync(id, ct);
        if (link == null)
        {
            return false;
        }

        await _links.DeleteAsync(link, ct);
        return true;
    }

    private static ProjectLinkDto Map(ProjectLink link)
    {
        return new ProjectLinkDto
        {
            Id = link.Id,
            ProjectId = link.ProjectId,
            Url = link.Url,
            Title = link.Title,
            Notes = link.Notes
        };
    }
}
