namespace Domain.Common;
public class EntityLog
{
    public int Id { get; set; }
    public string EntityType { get; set; } = null!;
    public string? EntityId { get; set; }
    public string Action { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string IpAddress { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public DateTime TimeStamp { get; set; }
    public string ActionResult { get; set; } = null!;
    public string? ErrorMessage { get; set; }
    public string? ErrorDescription { get; set; }
}