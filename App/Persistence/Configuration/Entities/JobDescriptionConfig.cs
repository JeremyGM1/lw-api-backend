using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.JobDescriptions;

namespace Persistence.Configuration.Entities;
public class JobDescriptionConfig : IEntityTypeConfiguration<JobDescription>
{
    public void Configure(EntityTypeBuilder<JobDescription> builder)
    {
        builder.HasKey(jd => jd.Id);
        builder.Property(jd => jd.Id).HasMaxLength(50);
        builder.Property(jd => jd.Title).HasMaxLength(100).IsRequired();
        builder.Property(jd => jd.Status).HasConversion<string>().IsRequired();
        builder.Property(jd => jd.Location).HasMaxLength(100).HasConversion<string>().IsRequired();
        builder.Property(jd => jd.PostingUrl).HasMaxLength(100).IsRequired();
        builder.Property(jd => jd.KeyResponsabilities).HasMaxLength(500).IsRequired();
        builder.Property(jd => jd.InternalNotes).HasMaxLength(500);

        builder.HasIndex(jd => jd.Title);
        builder.HasIndex(jd => jd.CreatedAt);
        builder.HasIndex(jd => jd.CreatedBy);
        builder.HasIndex(jd => jd.Status);
        builder.HasIndex(jd => jd.ApprovalDate);
        builder.HasIndex(r => new { r.Status, r.CreatedAt });
    }
}