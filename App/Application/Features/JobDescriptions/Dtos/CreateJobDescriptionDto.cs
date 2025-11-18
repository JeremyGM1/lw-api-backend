namespace Application.Features.JobDescriptions.Dtos;
public class CreateJobDescriptionDto
{
    public required string Title { get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string InternalNotes { get; set; }
}