namespace Application.Features.JobDescriptions.Dtos.Response;
public class UpdateJobDescriptionResponseDto
{
    public required string ReqCode { get; set; }
    public required string Title { get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string InternalNotes { get; set; }
    public List<string> RequiredSkills { get; set; } = null!;
    public List<string> DesiredSkills { get; set; } = null!;
    public List<string> BonusSkills { get; set; } = null!;
}