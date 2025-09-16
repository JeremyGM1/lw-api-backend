namespace Domain.Entities;
public class JobCandidate
{
    public int JobDescriptionId { get; set; }
    public required string CandidateId { get; set; }
    public virtual JobDescription JobDescription { get; set; } = null!;
    public virtual Candidate Candidate{ get; set; } = null!;
}
