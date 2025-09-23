using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.JobDescriptions;

namespace Persistence.Configuration.JobDescriptionConfig;

public class JobDescriptionStatusConfig : IEntityTypeConfiguration<JobDescriptionStatus>
{
    public void Configure(EntityTypeBuilder<JobDescriptionStatus> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Status);

        builder.HasData(
            new JobDescriptionStatus { Id = 1, Status = "Open" },
            new JobDescriptionStatus { Id = 2, Status = "Closed" },
            new JobDescriptionStatus { Id = 3, Status = "On-Hold" }
        );
    }
}