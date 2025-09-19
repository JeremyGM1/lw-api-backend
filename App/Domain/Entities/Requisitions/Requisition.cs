using Domain.Common;

namespace Domain.Entities.Requisitions;
public class Requisition : BaseEntity<string>
{
    public required string Name { get; set; }
    public int Headcount { get; set; }
    public required RequisitionStatus Status { get; set; }
    public required string Location { get; set; }
    public required RequisitionWorkType WorkType { get; set; }
    public required string Department { get; set; }
    public required string HiringManager { get; set; }
    public required string Owner { get; set; }
    public decimal CompensationBandMin { get; set; }
    public decimal CompensationBandMax { get; set; }
    public required string CompensationBandCurrency { get; set; }
    public required string CompensationBandInterval { get; set; }
    public string? Description { get; set; }
    public string? BackfillName { get; set; }
    public RequisitionHardware Hardware { get; set; }
    //public DateTime CreationDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public ICollection<RequisitionJob>? LinkedPostingList { get; set; } = [];
}
public enum RequisitionStatus
{
    Cancelled,
    Closed,
    Completed,
    OnHold,
    Open,
}
public enum RequisitionWorkType
{
    Contractor,
    FullTime,
    Internship,
    PartTime,
}
public enum RequisitionHardware
{
    Dell,
    Mac
}