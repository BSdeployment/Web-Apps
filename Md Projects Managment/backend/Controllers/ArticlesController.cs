using Backend.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/projects/{projectId:int}/article")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articles;

    public ArticlesController(IArticleService articles)
    {
        _articles = articles;
    }

    [HttpGet]
    public async Task<ActionResult<ArticleDto>> Get(int projectId, CancellationToken ct)
    {
        var article = await _articles.GetByProjectIdAsync(projectId, ct);
        if (article == null)
        {
            return NotFound(ApiErrors.NotFound($"Article for project {projectId} was not found."));
        }

        return Ok(article);
    }

    [HttpPut]
    public async Task<ActionResult<ArticleDto>> Upsert(int projectId, ArticleUpdateDto dto, CancellationToken ct)
    {
        var updated = await _articles.UpsertAsync(projectId, dto, ct);
        if (updated == null)
        {
            return NotFound(ApiErrors.NotFound($"Project {projectId} was not found."));
        }

        return Ok(updated);
    }
}
