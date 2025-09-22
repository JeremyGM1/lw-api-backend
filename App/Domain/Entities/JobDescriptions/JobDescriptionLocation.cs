namespace Domain.Entities.JobDescriptions;
public class JobDescriptionLocation
{
    public int Id { get; set; }
    public string Location { get; set; } = null!;
    public ICollection<JobDescription> JobDescriptions { get; set; } = [];
}