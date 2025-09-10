namespace Domain.Common;

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public virtual required TKey Id { get; set; } = default!;
    public virtual string CreatedBy { get; set; } = default!;
    public virtual DateTime CreatedAt { get; set; }
    public virtual string? UpdatedBy { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
}