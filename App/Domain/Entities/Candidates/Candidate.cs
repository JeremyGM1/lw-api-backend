using Domain.Common;

namespace Domain.Entities.Candidates;
public class Candidate : BaseEntity<string>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public ICollection<JobCandidate> JobCandidates { get; set; } = [];
}