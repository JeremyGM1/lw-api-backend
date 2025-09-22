using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;
public class RequisitionConfig : IEntityTypeConfiguration<Requisition>
{
    public void Configure(EntityTypeBuilder<Requisition> builder)
    {
        builder.HasKey(r   => r.Id);

        builder.HasOne(r => r.Status)
            .WithMany(s => s.Requisitions)
            .HasForeignKey(r => r.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.WorkType)
            .WithMany(s => s.Requisitions)
            .HasForeignKey(r => r.WorkTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Hardware)
            .WithMany(s => s.Requisitions)
            .HasForeignKey(r => r.HardwareId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.Name).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Headcount).IsRequired();
        builder.Property(r => r.Location).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Department).HasMaxLength(100).IsRequired();
        builder.Property(r => r.HiringManager).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Owner).HasMaxLength(100).IsRequired();
        builder.Property(r => r.CompensationBandMin).HasPrecision(20, 4).IsRequired();
        builder.Property(r => r.CompensationBandMax).HasPrecision(20, 4).IsRequired();
        builder.Property(r => r.CompensationBandCurrency).HasMaxLength(10).IsRequired();
        builder.Property(r => r.CompensationBandInterval).HasMaxLength(10).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(200);

        builder.HasIndex(r => r.StatusId);
        builder.HasIndex(r => r.WorkTypeId);
        builder.HasIndex(r => r.HiringManager);
        builder.HasIndex(r => r.CreatedAt);
        builder.HasIndex(r => r.CreatedBy);
        builder.HasIndex(r => new { r.StatusId, r.CreatedAt });
    }
}