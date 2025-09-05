namespace Domain.AccessControl;

public abstract class BaseEntity : Audit
{
    public virtual string Id { get; set; } = Guid.NewGuid().ToString();
    public bool IsActive { get; set; } = true;
    public void ToggleActive()
    {
        IsActive = !IsActive;
    }
}
