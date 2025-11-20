namespace Application.Features.JobDescriptions.Dtos.Request;

public class UpdateJobDescriptionDto
{
    public string? Title { get; set; }
    public string? PostingUrl { get; set; }
    public string? KeyResponsabilities { get; set; }
    public string? InternalNotes { get; set; }
}