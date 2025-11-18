using Domain.Entities.JobDescriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.JobDescriptionConfig;
public class JobDescriptionLogConfig : IEntityTypeConfiguration<JobDescriptionLog>
{
    public void Configure(EntityTypeBuilder<JobDescriptionLog> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.EntityId);
        builder.HasIndex(e => e.TimeStamp);
    }
}