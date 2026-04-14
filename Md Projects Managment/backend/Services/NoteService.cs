using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public enum NoteSaveResultType
{
    Saved,
    Deleted,
    NotFound
}

public record NoteSaveResult(NoteSaveResultType Result, ProjectNoteDto? Note);

public interface INoteService
{
    Task<ProjectNoteDto?> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<NoteSaveResult> UpsertAsync(int projectId, ProjectNoteUpdateDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int projectId, CancellationToken ct);
}

public class NoteService : INoteService
{
    private readonly INoteRepository _notes;
    private readonly IProjectRepository _projects;

    public NoteService(INoteRepository notes, IProjectRepository projects)
    {
        _notes = notes;
        _projects = projects;
    }

    public async Task<ProjectNoteDto?> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        var note = await _notes.GetByProjectIdAsync(projectId, ct);
        return note == null ? null : Map(note);
    }

    public async Task<NoteSaveResult> UpsertAsync(int projectId, ProjectNoteUpdateDto dto, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(projectId, ct);
        if (project == null)
        {
            return new NoteSaveResult(NoteSaveResultType.NotFound, null);
        }

        var content = dto.Content?.Trim() ?? string.Empty;
        var existing = await _notes.GetByProjectIdAsync(projectId, ct);

        if (string.IsNullOrWhiteSpace(content))
        {
            if (existing != null)
            {
                await _notes.DeleteAsync(existing, ct);
            }
            return new NoteSaveResult(NoteSaveResultType.Deleted, null);
        }

        if (existing == null)
        {
            var created = new ProjectNote
            {
                ProjectId = projectId,
                Content = content,
                UpdatedAt = DateTime.UtcNow
            };
            await _notes.AddAsync(created, ct);
            return new NoteSaveResult(NoteSaveResultType.Saved, Map(created));
        }

        existing.Content = content;
        existing.UpdatedAt = DateTime.UtcNow;
        await _notes.UpdateAsync(ct);
        return new NoteSaveResult(NoteSaveResultType.Saved, Map(existing));
    }

    public async Task<bool> DeleteAsync(int projectId, CancellationToken ct)
    {
        var note = await _notes.GetByProjectIdAsync(projectId, ct);
        if (note == null)
        {
            return false;
        }

        await _notes.DeleteAsync(note, ct);
        return true;
    }

    private static ProjectNoteDto Map(ProjectNote note)
    {
        return new ProjectNoteDto
        {
            Id = note.Id,
            ProjectId = note.ProjectId,
            Content = note.Content,
            UpdatedAt = note.UpdatedAt
        };
    }
}
