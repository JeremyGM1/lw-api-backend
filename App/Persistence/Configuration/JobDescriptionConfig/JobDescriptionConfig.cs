using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.JobDescriptions;

namespace Persistence.Configuration.JobDescriptionConfig;
public class JobDescriptionConfig : IEntityTypeConfiguration<JobDescription>
{
    public void Configure(EntityTypeBuilder<JobDescription> builder)
    {
        builder.HasKey(jd => jd.Id);

        builder.HasOne(jd => jd.Status)
            .WithMany(s => s.JobDescriptions)
            .HasForeignKey(jd => jd.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(jd => jd.Location)
            .WithMany(l => l.JobDescriptions)
            .HasForeignKey(jd => jd.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(jd => jd.Title).HasMaxLength(100).IsRequired();
        builder.Property(jd => jd.PostingUrl).HasMaxLength(100).IsRequired();
        builder.Property(jd => jd.KeyResponsabilities).HasMaxLength(500).IsRequired();
        builder.Property(jd => jd.InternalNotes).HasMaxLength(500);

        builder.HasIndex(jd => jd.Title);
        builder.HasIndex(jd => jd.CreatedAt);
        builder.HasIndex(jd => jd.CreatedBy);
        builder.HasIndex(jd => jd.StatusId);
        builder.HasIndex(jd => jd.LocationId);
        builder.HasIndex(jd => jd.ApprovalDate);
        builder.HasIndex(r => new { r.StatusId, r.CreatedAt });
    }
}