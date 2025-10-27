namespace Domain.Entities.Requisitions;
public class RequisitionLocation
{
    public int Id { get; set; }
    public string Location { get; set; } = null!;
    public ICollection<Requisition> Requisitions { get; set; } = [];
}