using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public interface IArticleRepository
{
    Task<Article?> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<Article> AddAsync(Article article, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}

public class ArticleRepository : IArticleRepository
{
    private readonly WritingDbContext _db;

    public ArticleRepository(WritingDbContext db)
    {
        _db = db;
    }

    public Task<Article?> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        // ✅ בלי AsNoTracking → EF עוקב אחרי הישות
        return _db.Articles
            .FirstOrDefaultAsync(a => a.ProjectId == projectId, ct);
    }

    public async Task<Article> AddAsync(Article article, CancellationToken ct)
    {
        _db.Articles.Add(article);
        await _db.SaveChangesAsync(ct);
        return article;
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return _db.SaveChangesAsync(ct);
    }
}