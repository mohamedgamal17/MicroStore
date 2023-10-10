using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Ordering.Application.Domain;

namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderSalesReportEntityTypeConfiguration : IEntityTypeConfiguration<OrderSalesReport>
    {
        public void Configure(EntityTypeBuilder<OrderSalesReport> builder)
        {
            builder.ToTable("vw_OrderSalesReports");
            builder.HasKey(x=> new {x.Date, x.CurrentState});
            builder.Property(x => x.CurrentState);
            builder.Property(x => x.Date);
            builder.Property(x => x.TotalOrders);
            builder.Property(x => x.TotalPrice);
            builder.Property(x => x.TotalShippingPrice);
            builder.Property(x => x.TotalTaxPrice);                
        }
    }
}
