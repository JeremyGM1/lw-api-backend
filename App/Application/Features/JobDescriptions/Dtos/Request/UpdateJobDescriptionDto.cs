using System.ComponentModel.DataAnnotations;

namespace Application.Features.JobDescriptions.Dtos.Request;

public class UpdateJobDescriptionDto
{
    [StringLength(100)]
    public string? ReqCode { get; set; }
    [StringLength(100)]
    public string? Title { get; set; }
    [StringLength(500)]
    public string? PostingUrl { get; set; }
    [StringLength(500)]
    public string? KeyResponsabilities { get; set; }
    [StringLength(500)]
    public string? InternalNotes { get; set; }
    public required List<string> RequiredSkills { get; set; }
    public required List<string> DesiredSkills { get; set; }
    public required List<string> BonusSkills { get; set; }
}