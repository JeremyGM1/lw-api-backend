namespace Domain.Entities.Requisitions;
public class RequisitionLog
{
    public int Id { get; set; }
    public string RequisitionId { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public virtual Requisition Requisition { get; set; } = null!;
}