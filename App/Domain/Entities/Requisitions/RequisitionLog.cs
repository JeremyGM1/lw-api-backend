namespace Domain.Entities.Requisitions;

public class RequisitionLog
{
    public int Id { get; set; }
    public string? RequisitionId { get; set; }
    public string Action { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string IpAddress { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public string ActionResult { get; set; } = null!;
    public string? ErrorMessage { get; set; }

    public virtual Requisition Requisition { get; set; } = null!;
}