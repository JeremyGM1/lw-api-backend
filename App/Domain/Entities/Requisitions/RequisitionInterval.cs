namespace Domain.Entities.Requisitions;
public class RequisitionInterval
{
    public int Id { get; set; }
    public string Interval { get; set; } = null!;
    public ICollection<Requisition> Requisitions { get; set; } = [];
}