namespace Domain.Entities.JobDescriptions;

public class JobDescriptionStatus
{
    public int Id { get; set; }
    public string Status { get; set; } = null!;
    public ICollection<JobDescription> JobDescriptions { get; set; } = [];
}