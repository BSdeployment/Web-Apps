using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class ProjectNoteDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public class ProjectNoteUpdateDto
{
    [Required]
    public string Content { get; set; } = string.Empty;
}
