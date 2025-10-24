using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;

public class RequisitionCurrencyConfig : IEntityTypeConfiguration<RequisitionCurrency>
{
    public void Configure(EntityTypeBuilder<RequisitionCurrency> builder)
    {
        builder.HasKey(wt => wt.Id);
        builder.Property(wt => wt.Currency).IsRequired();

        builder.HasData(
            new RequisitionCurrency { Id = 1, Currency = "COP" },
            new RequisitionCurrency { Id = 2, Currency = "USD" },
            new RequisitionCurrency { Id = 3, Currency = "CAD" },
            new RequisitionCurrency { Id = 4, Currency = "PHP" },
            new RequisitionCurrency { Id = 5, Currency = "EUR" }
        );
    }
}