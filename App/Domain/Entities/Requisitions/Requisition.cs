using Domain.Common;
using Domain.Entities.JobDescriptions;

namespace Domain.Entities.Requisitions;

public class Requisition : BaseEntity<int>
{
    public required string ReqCode { get; set; }
    public required string Name { get; set; }
    public int StatusId { get; set; }
    public int WorkTypeId { get; set; }
    public int HardwareId { get; set; }
    public int LocationId { get; set; }
    public int LocationTypeId { get; set; }
    public int Headcount { get; set; }
    public required RequisitionStatus Status { get; set; }
    public required RequisitionLocation Location { get; set; }
    public required RequisitionLocationType LocationType { get; set; }
    public required RequisitionWorkType WorkType { get; set; }
    public required string Department { get; set; }
    public required string HiringManager { get; set; }
    public required string Owner { get; set; }
    public decimal CompensationBandMin { get; set; }
    public decimal CompensationBandMax { get; set; }
    public int CompensationBandCurrencyId { get; set; }
    public int CompensationBandIntervalId { get; set; }
    public required RequisitionCurrency CompensationBandCurrency { get; set; }
    public required RequisitionInterval CompensationBandInterval { get; set; }
    public string? Description { get; set; }
    public string? BackfillName { get; set; }
    public required RequisitionHardware Hardware { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public ICollection<JobDescription> JobDescriptions { get; set; } = [];

    public void AddJobDescription(JobDescription jobDescription)
    {
        jobDescription.ReqCode = ReqCode;
        jobDescription.Requisition = this;
        JobDescriptions.Add(jobDescription);
    }


    public void RemoveJobDescription(JobDescription jobDescription)
    {
        jobDescription.ReqCode = ReqCode;
        jobDescription.Requisition = this;
        JobDescriptions.Remove(jobDescription);
    }
}