namespace Domain.Common;

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public required TKey Id { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}