namespace Domain.Entities.Requisitions;
public class RequisitionStatus
{
    public int Id { get; set; }
    public string Status { get; set; } = null!;
    public ICollection<Requisition> Requisitions { get; set; } = [];
}