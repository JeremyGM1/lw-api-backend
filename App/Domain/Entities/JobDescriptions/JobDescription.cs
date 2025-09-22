using Domain.Common;
using Domain.Entities.Candidates;
using Domain.Entities.Requisitions;

namespace Domain.Entities.JobDescriptions;
public class JobDescription : BaseEntity<int>
{
    public override required int Id { get; set; }
    public required string Title { get; set; }
    public required JobDescriptionStatus Status { get; set; } //Open,Closed,OnHold
    public required JobDescriptionLocation Location { get; set; } //OnSite,Hybrid,Remote    
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