namespace Domain.Entities;
public class RequisitionJob
{
    public required string ReqId { get; set; }
    public required string JobDescriptionId { get; set; }
    public Requisition Requisitions { get; set; } = null!;
    public JobDescription JobDescriptions { get; set; } = null!;
}