namespace Application.Features.JobDescriptions.Dtos;

public class UpdateJobDescriptionDto
{
    public string? Title { get; set; }
    public int StatusId { get; set; }
    public int LocationTypeId { get; set; }
    public string? PostingUrl { get; set; }
    public string? InternalNotes { get; set; }
}