using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.JobDescriptions;

namespace Persistence.Configuration.JobDescriptionConfig;

public class JobDescriptionLocationConfig : IEntityTypeConfiguration<JobDescriptionLocation>
{
    public void Configure(EntityTypeBuilder<JobDescriptionLocation> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Location);

        builder.HasData(
            new JobDescriptionLocation { Id = 1, Location = "On-Site" },
            new JobDescriptionLocation { Id = 2, Location = "Hybrid" },
            new JobDescriptionLocation { Id = 3, Location = "Remote" }
        );
    }
}