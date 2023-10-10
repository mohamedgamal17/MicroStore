using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Ordering.Application.Domain;

namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductSalesReportEntityTypeConfiguration : IEntityTypeConfiguration<ProductSalesReport>
    {
        public void Configure(EntityTypeBuilder<ProductSalesReport> builder)
        {
            builder.ToTable("vw_ProductSalesReports");
            builder.HasKey(x => new { x.ProductId, x.Date });
            builder.Property(x => x.Quantity);
            builder.Property(x => x.TotalPrice);
        }
    }
}
