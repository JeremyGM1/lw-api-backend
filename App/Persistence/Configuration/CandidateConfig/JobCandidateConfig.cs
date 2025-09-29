using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Candidates;

namespace Persistence.Configuration.Candidates;

public class JobCandidateConfig : IEntityTypeConfiguration<JobCandidate>
{
    public void Configure(EntityTypeBuilder<JobCandidate> builder)
    {
        builder.HasKey(jc => new { jc.JobDescriptionId, jc.CandidateId });

        builder.HasOne(jc => jc.JobDescription)
            .WithMany(jc => jc.LinkedCandidatesList)
            .HasForeignKey(jc => jc.JobDescriptionId);

        builder.HasOne(jc => jc.Candidate)
            .WithMany(jc => jc.JobCandidates)
            .HasForeignKey(jc => jc.CandidateId);
    }
}