using Backend.Models;

namespace Backend.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync(CancellationToken ct);
    Task<Project?> GetByIdAsync(int id, CancellationToken ct);
    Task<Project> AddAsync(Project project, CancellationToken ct);
    Task UpdateAsync(Project project, CancellationToken ct);
    Task DeleteAsync(Project project, CancellationToken ct);
}
