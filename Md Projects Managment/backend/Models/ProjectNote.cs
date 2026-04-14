using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class ProjectNote
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }

    public Project Project { get; set; } = null!;
}
