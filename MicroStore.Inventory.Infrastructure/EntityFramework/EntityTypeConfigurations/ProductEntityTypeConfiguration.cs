using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.Stock);

            builder.Property(x => x.AllocatedStock);

        }
    }
}
