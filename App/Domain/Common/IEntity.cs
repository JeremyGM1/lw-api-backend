namespace Domain.Common;

public interface IEntity<TypeKey>
{
    public TypeKey Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}