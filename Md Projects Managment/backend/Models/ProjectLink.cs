using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class ProjectLink
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [Required]
    [MaxLength(2048)]
    public string Url { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public Project Project { get; set; } = null!;
}
