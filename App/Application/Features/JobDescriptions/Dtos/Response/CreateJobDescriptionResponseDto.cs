using System.ComponentModel.DataAnnotations;

namespace Application.Features.JobDescriptions.Dtos.Response;

public class CreateJobDescriptionResponseDto
{
    public int Id { get; set; }
    [StringLength(6)]
    public required string ReqCode { get; set; }
    [StringLength(100, MinimumLength = 1)]
    public required string Title { get; set; }
    [StringLength(500, MinimumLength = 1)]
    public required string PostingUrl { get; set; }
    [StringLength(500, MinimumLength = 1)]
    public required string KeyResponsabilities { get; set; }
    [StringLength(500, MinimumLength = 1)]
    public required string InternalNotes { get; set; }
    [StringLength(500, MinimumLength = 1)]
    public List<string> RequiredSkills { get; set; } = null!;
    public List<string> DesiredSkills { get; set; } = null!;
    public List<string> BonusSkills { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string LocationType { get; set; } = null!;
    public string? CreatedBy { get; set; }
    public string? CreatedAt { get; set; }
}