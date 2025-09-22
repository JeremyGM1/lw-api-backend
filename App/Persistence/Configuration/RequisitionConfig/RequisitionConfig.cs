using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;
public class RequisitionConfig : IEntityTypeConfiguration<Requisition>
{
    public void Configure(EntityTypeBuilder<Requisition> builder)
    {
        builder.HasKey(r   => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50);
        builder.Property(r => r.Name).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Headcount).HasMaxLength(255).IsRequired();
        builder.Property(r => r.Status).HasConversion<string>().IsRequired();
        builder.Property(r => r.Location).HasMaxLength(100).IsRequired();
        builder.Property(r => r.WorkType).HasConversion<string>().IsRequired();
        builder.Property(r => r.Department).HasMaxLength(100).IsRequired();
        builder.Property(r => r.HiringManager).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Owner).HasMaxLength(100).IsRequired();
        builder.Property(r => r.CompensationBandMin).HasPrecision(20, 4).IsRequired();
        builder.Property(r => r.CompensationBandMax).HasPrecision(20, 4).IsRequired();
        builder.Property(r => r.CompensationBandCurrency).HasMaxLength(10).IsRequired();
        builder.Property(r => r.CompensationBandInterval).HasMaxLength(10).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(200).IsRequired();
        builder.Property(r => r.Hardware).HasConversion<string>().IsRequired();

        builder.HasIndex(r => r.Id);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.WorkType);
        builder.HasIndex(r => r.HiringManager);
        builder.HasIndex(r => r.CreatedAt);
        builder.HasIndex(r => r.CreatedBy);
        builder.HasIndex(r => new { r.Status, r.CreatedAt });
    }
}