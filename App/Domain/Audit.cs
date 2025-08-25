
namespace Domain
{
    public class Audit
    {
        public required string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public required string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}