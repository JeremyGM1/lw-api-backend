using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.Relationships;
public class RequisitionJobConfig : IEntityTypeConfiguration<RequisitionJob>
{
    public void Configure(EntityTypeBuilder<RequisitionJob> builder)
    {
        builder.HasKey(rj => new { rj.ReqId, rj.JobDescriptionId });

        builder.HasOne(rj => rj.Requisition)
            .WithMany(rj => rj.LinkedPostingList)
            .HasForeignKey(rj => rj.ReqId);

        builder.HasOne(rj => rj.JobDescription)
            .WithMany(rj => rj.LinkedPostingList)
            .HasForeignKey(rj => rj.JobDescriptionId);
    }
}