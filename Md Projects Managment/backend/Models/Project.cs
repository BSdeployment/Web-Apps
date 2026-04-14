using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Project
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Completed { get; set; }

    public Article? Article { get; set; }

    public ProjectNote? Note { get; set; }

    public List<ProjectFile> Files { get; set; } = new();

    public List<ProjectLink> Links { get; set; } = new();
}
