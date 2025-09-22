using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.JobDescriptions;

namespace Persistence.Configuration.JobDescriptionConfig;
public class JobSkillConfig : IEntityTypeConfiguration<JobSkill>
{
    public void Configure(EntityTypeBuilder<JobSkill> builder)
    {
        builder.HasKey(js => new { js.JobId, js.SkillId });

        builder.HasOne(js => js.JobDescription)
            .WithMany(js => js.JobSkills)
            .HasForeignKey(js => js.JobId);

        builder.HasOne(js => js.Skill)
            .WithMany(js => js.JobSkills)
            .HasForeignKey(js => js.SkillId);
    }
}