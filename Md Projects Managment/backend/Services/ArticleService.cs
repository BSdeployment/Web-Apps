using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public interface IArticleService
{
    Task<ArticleDto?> GetByProjectIdAsync(int projectId, CancellationToken ct);
    Task<ArticleDto?> UpsertAsync(int projectId, ArticleUpdateDto dto, CancellationToken ct);
}

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articles;
    private readonly IProjectRepository _projects;

    public ArticleService(IArticleRepository articles, IProjectRepository projects)
    {
        _articles = articles;
        _projects = projects;
    }

    public async Task<ArticleDto?> GetByProjectIdAsync(int projectId, CancellationToken ct)
    {
        var article = await _articles.GetByProjectIdAsync(projectId, ct);
        return article == null ? null : Map(article);
    }

    public async Task<ArticleDto?> UpsertAsync(int projectId, ArticleUpdateDto dto, CancellationToken ct)
    {
        var project = await _projects.GetByIdAsync(projectId, ct);
        if (project == null)
        {
            return null;
        }

        var existing = await _articles.GetByProjectIdAsync(projectId, ct);

        if (existing == null)
        {
            var created = new Article
            {
                ProjectId = projectId,
                MarkdownContent = dto.MarkdownContent,
                UpdatedAt = DateTime.UtcNow
            };

            await _articles.AddAsync(created, ct);
            return Map(created);
        }

        // ✅ עדכון ישות קיימת (Tracked)
        existing.MarkdownContent = dto.MarkdownContent;
        existing.UpdatedAt = DateTime.UtcNow;

        // ❗ אין Update
        await _articles.SaveChangesAsync(ct);

        return Map(existing);
    }

    private static ArticleDto Map(Article article)
    {
        return new ArticleDto
        {
            Id = article.Id,
            ProjectId = article.ProjectId,
            MarkdownContent = article.MarkdownContent,
            UpdatedAt = article.UpdatedAt
        };
    }
}