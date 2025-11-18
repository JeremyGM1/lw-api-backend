namespace Application.Features.JobDescriptions.Dtos.Response;
public class GetJobDescriptionResponseDto
{
    public required string ReqCode { get; set; }
    public required string Title { get; set; }
    public required string Status{ get; set; }
    public required string LocationType{ get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string InternalNotes{ get; set; }
    public DateTime? ApprovalDate { get; set; }
}