
namespace Domain;

public class Audit
{
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}