namespace Domain.Common;

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public virtual required TKey Id { get; set; }
    public required string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}