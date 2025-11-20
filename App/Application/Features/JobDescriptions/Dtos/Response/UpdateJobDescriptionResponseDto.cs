namespace Application.Features.JobDescriptions.Dtos.Response;
public class UpdateJobDescriptionResponseDto
{
    public required string ReqCode { get; set; }
    public required string Title { get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string InternalNotes { get; set; }
}