namespace Domain.Entities.Requisitions;
public class RequisitionCurrency
{
    public int Id { get; set; }
    public string Currency { get; set; } = null!;
    public ICollection<Requisition> Requisitions { get; set; } = [];
}