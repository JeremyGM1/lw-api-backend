namespace Application.Features.JobDescriptions.Dtos.Response;
public class GetJobDescriptionResponseDto
{
    public int Id { get; init; }
    public required string ReqCode { get; init; }
    public required string Title { get; init; }
    public required string Status{ get; init; }
    public required string LocationType{ get; init; }
    public required string PostingUrl { get; init; }
    public required string KeyResponsabilities { get; init; }
    public required string InternalNotes{ get; init; }
    public DateTime? ApprovalDate { get; init; }
}