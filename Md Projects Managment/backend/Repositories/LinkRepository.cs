using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public interface ILinkRepository
{
    Task<List<ProjectLink>> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<ProjectLink?> GetByIdAsync(int id, CancellationToken ct);
    Task<ProjectLink> AddAsync(ProjectLink link, CancellationToken ct);
    Task DeleteAsync(ProjectLink link, CancellationToken ct);
}

public class LinkRepository : ILinkRepository
{
    private readonly WritingDbContext _db;

    public LinkRepository(WritingDbContext db)
    {
        _db = db;
    }

    public Task<List<ProjectLink>> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        return _db.ProjectLinks.AsNoTracking().Where(l => l.ProjectId == projectId).ToListAsync(ct);
    }

    public Task<ProjectLink?> GetByIdAsync(int id, CancellationToken ct)
    {
        return _db.ProjectLinks.FirstOrDefaultAsync(l => l.Id == id, ct);
    }

    public async Task<ProjectLink> AddAsync(ProjectLink link, CancellationToken ct)
    {
        _db.ProjectLinks.Add(link);
        await _db.SaveChangesAsync(ct);
        return link;
    }

    public async Task DeleteAsync(ProjectLink link, CancellationToken ct)
    {
        _db.ProjectLinks.Remove(link);
        await _db.SaveChangesAsync(ct);
    }
}
