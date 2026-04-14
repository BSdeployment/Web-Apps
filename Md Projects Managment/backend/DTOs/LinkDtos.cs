using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class ProjectLinkDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Notes { get; set; }
}

public class ProjectLinkCreateDto
{
    [Required]
    [MaxLength(2048)]
    public string Url { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}
