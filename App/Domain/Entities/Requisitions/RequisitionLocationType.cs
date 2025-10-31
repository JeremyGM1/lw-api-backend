namespace Domain.Entities.Requisitions;

public class RequisitionLocationType
{
    public int Id { get; set; }
    public string LocationType { get; set; } = null!;
    public ICollection<Requisition> Requisitions { get; set; } = [];
}