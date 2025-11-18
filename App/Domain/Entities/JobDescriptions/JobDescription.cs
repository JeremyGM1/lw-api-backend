using Domain.Common;
using Domain.Entities.Candidates;
using Domain.Entities.Requisitions;

namespace Domain.Entities.JobDescriptions;

public class JobDescription : BaseEntity<int>
{
    public required string Title { get; set; }
    public required string RequisitionCode { get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string? InternalNotes { get; set; }
    public required DateTime? ApprovalDate { get; set; }
    public required Requisition Requisition { get; set; }

    public required ICollection<JobSkill> JobSkills { get; set; } = [];
    public required ICollection<JobCandidate> LinkedCandidatesList { get; set; } = [];
}