using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Inventory.Domain.OrderAggregate;
namespace MicroStore.Inventory.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.ExternalItemId).HasMaxLength(256);

            builder.Property(x => x.ExternalProductId).HasMaxLength(256);

            builder.Property(x => x.Name).HasMaxLength(300);

            builder.Property(x => x.Sku).HasMaxLength(256);

            builder.Property(x => x.Thumbnail).HasMaxLength(500);

            builder.HasIndex(x => x.ExternalItemId).IsUnique();

            builder.HasIndex(x => x.ExternalProductId);

            builder.HasIndex(x => x.Name);

            builder.HasIndex(x => x.Sku);
        }
    }
}
