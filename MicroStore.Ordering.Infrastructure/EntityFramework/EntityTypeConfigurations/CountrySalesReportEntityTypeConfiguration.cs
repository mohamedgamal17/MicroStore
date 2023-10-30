using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Ordering.Application.Domain;

namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class CountrySalesReportEntityTypeConfiguration : IEntityTypeConfiguration<CountrySalesReport>
    {
        public void Configure(EntityTypeBuilder<CountrySalesReport> builder)
        {
            builder.ToTable("vw_CountrySalesReports");

            builder.HasNoKey();

            builder.Property(x => x.TotalOrders);

            builder.Property(x => x.CountryCode);

            builder.Property(x => x.TotalShippingPrice);

            builder.Property(x => x.TotalTaxPrice);

            builder.Property(x => x.TotalTaxPrice);

            builder.Property(x => x.Date);

        }
    }
}
