using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Inventory.Domain.OrderAggregate;
namespace MicroStore.Inventory.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.ProductId).HasMaxLength(256);

            builder.Property(x => x.Name).HasMaxLength(300);

            builder.Property(x => x.Sku).HasMaxLength(256);

            builder.Property(x => x.Thumbnail).HasMaxLength(500);

            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => x.Name);

            builder.HasIndex(x => x.Sku);
        }
    }
}
