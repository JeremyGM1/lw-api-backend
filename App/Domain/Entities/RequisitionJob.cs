namespace Domain.Entities;
public class RequisitionJob
{
    public required string ReqId { get; set; }
    public required string JobDescriptionId { get; set; }
    public Requisition Requisition { get; set; } = null!;
    public JobDescription JobDescription { get; set; } = null!;
}