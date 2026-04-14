using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class ArticleDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string MarkdownContent { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public class ArticleUpdateDto
{
    [Required]
    public string MarkdownContent { get; set; } = string.Empty;
}
