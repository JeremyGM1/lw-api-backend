namespace Application.Features.JobDescriptions.Dtos.Response;

public class CreateJobDescriptionResponseDto
{
    public int Id { get; set; }
    public required string ReqCode { get; set; }
    public required string Title { get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string InternalNotes { get; set; }
    public string Status { get; set; } = null!;
    public string LocationType { get; set; } = null!;
    public DateOnly ApprovalDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}