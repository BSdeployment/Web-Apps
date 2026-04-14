using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Article
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [Required]
    public string MarkdownContent { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }

    public Project Project { get; set; } = null!;
}
