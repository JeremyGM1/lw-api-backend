using Domain.Common;
using Domain.Entities.Candidates;
using Domain.Entities.Requisitions;

namespace Domain.Entities.JobDescriptions;

public class JobDescription : BaseEntity<int>
{
    public override required int Id { get; set; }
    public required string Title { get; set; }

    public int StatusId { get; set; }
    public int LocationId { get; set; }
    public JobDescriptionStatus Status { get; set; } = null!;
    public JobDescriptionLocation Location { get; set; } = null!;
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string? InternalNotes { get; set; }
    public required DateTime? ApprovalDate { get; set; }
    public required ICollection<JobSkill> JobSkills { get; set; } = [];
    public required ICollection<RequisitionJob> LinkedPostingList { get; set; } = [];
    public required ICollection<JobCandidate> LinkedCandidatesList { get; set; } = [];
}