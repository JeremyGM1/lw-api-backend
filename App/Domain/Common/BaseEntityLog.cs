namespace Domain.Common;
public abstract class BaseEntityLog
{
    public long Id { get; set; }
    public required string EntityType { get; set; }
    public string? EntityId { get; set; }
    public required string Action { get; set; }
    public required string UserEmail { get; set; }
    public required string IpAddress { get; set; }
    public required string UserAgent { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required string ActionResult { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorDescription { get; set; }
}