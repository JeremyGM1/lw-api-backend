using Domain.Common;

namespace Domain.Entities;
public class JobDescription : BaseEntity<int>
{
    public override required int Id { get; set; }
    public required string Title { get; set; }
    public required JobDescriptionStatus Status { get; set; }
    public required JobDescriptionLocation Location { get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    //public required Skill RequiredSkills { get; set; }
    //public required Skill DesiredSkills { get; set; }
    //public required Skill BonusSkills { get; set; }
    public required string? InternalNotes { get; set; }
    public required DateTime? ApprovalDate { get; set; }
    public required ICollection<JobSkill> JobSkills { get; set; } = [];
    public required ICollection<RequisitionJob> LinkedPostingList { get; set; } = [];
    public required ICollection<JobCandidate> LinkedCandidatesList { get; set; } = [];
}
public enum JobDescriptionLocation
{
    OnSite,
    Hybrid,
    Remote
}
public enum JobDescriptionStatus
{
    Open,
    Closed,
    OnHold
}