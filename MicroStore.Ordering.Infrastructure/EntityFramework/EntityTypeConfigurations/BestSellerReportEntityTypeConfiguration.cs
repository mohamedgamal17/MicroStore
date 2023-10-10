using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Ordering.Application.Domain;

namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class BestSellerReportEntityTypeConfiguration : IEntityTypeConfiguration<BestSellerReport>
    {
        public void Configure(EntityTypeBuilder<BestSellerReport> builder)
        {
            builder.ToTable("vw_BestSellerProducts");

            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.Quantity);

            builder.Property(x => x.Amount);
        }
    }
}
