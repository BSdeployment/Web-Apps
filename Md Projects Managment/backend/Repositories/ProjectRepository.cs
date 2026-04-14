using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly WritingDbContext _db;

    public ProjectRepository(WritingDbContext db)
    {
        _db = db;
    }

    public Task<List<Project>> GetAllAsync(CancellationToken ct)
    {
        return _db.Projects.AsNoTracking().OrderByDescending(p => p.CreatedAt).ToListAsync(ct);
    }

    public Task<Project?> GetByIdAsync(int id, CancellationToken ct)
    {
        return _db.Projects
            .Include(p => p.Article)
            .Include(p => p.Note)
            .Include(p => p.Files)
            .Include(p => p.Links)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken ct)
    {
        _db.Projects.Add(project);
        await _db.SaveChangesAsync(ct);
        return project;
    }

    public async Task UpdateAsync(Project project, CancellationToken ct)
    {
        _db.Projects.Update(project);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Project project, CancellationToken ct)
    {
        _db.Projects.Remove(project);
        await _db.SaveChangesAsync(ct);
    }
}
