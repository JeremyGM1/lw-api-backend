namespace Domain.Entities.Requisitions;

public class RequisitionHardware
{
    public int Id { get; set; }
    public string Hardware { get; set; } = null!;
    public ICollection<Requisition> Requisitions { get; set; } = [];
}