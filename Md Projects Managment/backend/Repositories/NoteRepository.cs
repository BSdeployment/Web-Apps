using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public interface INoteRepository
{
    Task<ProjectNote?> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<ProjectNote> AddAsync(ProjectNote note, CancellationToken ct);
    Task UpdateAsync(CancellationToken ct);
    Task DeleteAsync(ProjectNote note, CancellationToken ct);
}

public class NoteRepository : INoteRepository
{
    private readonly WritingDbContext _db;

    public NoteRepository(WritingDbContext db)
    {
        _db = db;
    }

    public Task<ProjectNote?> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        // ללא AsNoTracking כדי שהישות תהיה במעקב
        return _db.ProjectNotes
            .FirstOrDefaultAsync(n => n.ProjectId == projectId, ct);
    }

    public async Task<ProjectNote> AddAsync(ProjectNote note, CancellationToken ct)
    {
        _db.ProjectNotes.Add(note);
        await _db.SaveChangesAsync(ct);
        return note;
    }

    public async Task UpdateAsync(CancellationToken ct)
    {
        // אין צורך ב Update()
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(ProjectNote note, CancellationToken ct)
    {
        _db.ProjectNotes.Remove(note);
        await _db.SaveChangesAsync(ct);
    }
}