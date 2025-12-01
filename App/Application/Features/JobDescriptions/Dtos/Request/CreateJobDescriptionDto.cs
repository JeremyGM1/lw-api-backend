using System.ComponentModel.DataAnnotations;

namespace Application.Features.JobDescriptions.Dtos.Request;

public class CreateJobDescriptionDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Title { get; set; }
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public required string PostingUrl { get; set; }
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public required string KeyResponsabilities { get; set; }
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public required string InternalNotes { get; set; }
}