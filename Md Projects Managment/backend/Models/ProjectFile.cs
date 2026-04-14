using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class ProjectFile
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FileType { get; set; }

    public Project Project { get; set; } = null!;
}
