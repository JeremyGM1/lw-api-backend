using System.ComponentModel.DataAnnotations;

namespace Application.Features.JobDescriptions.Dtos.Response;
public class GetJobDescriptionResponseDto
{
    public int Id { get; init; }
    [StringLength(6)]
    public required string ReqCode { get; init; }
    [StringLength(100)]
    public required string Title { get; init; }
    [StringLength(500)]
    public required string Status{ get; init; }
    [StringLength(500)]
    public required string LocationType{ get; init; }
    [StringLength(500)]
    public required string PostingUrl { get; init; }
    [StringLength(500)]
    public required string KeyResponsabilities { get; init; }
    [StringLength(500)]
    public required string InternalNotes{ get; init; }
    [DataType(DataType.Date)]
    public DateTime? ApprovalDate { get; init; }
}