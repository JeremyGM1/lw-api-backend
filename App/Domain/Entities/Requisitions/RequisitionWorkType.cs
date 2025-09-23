namespace Domain.Entities.Requisitions;

public class RequisitionWorkType
{
    public int Id { get; set; }
    public string WorkType { get; set; } = null!;
    public ICollection<Requisition> Requisitions { get; set; } = [];
}