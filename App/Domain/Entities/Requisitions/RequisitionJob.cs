using Domain.Entities.JobDescriptions;

namespace Domain.Entities.Requisitions;

public class RequisitionJob
{
    public required string ReqId { get; set; }
    public required int JobDescriptionId { get; set; }
    public Requisition Requisition { get; set; } = null!;
    public JobDescription JobDescription { get; set; } = null!;
}