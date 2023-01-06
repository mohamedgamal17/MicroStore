using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MicroStore.Ordering.Application.Abstractions.StateMachines;

namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {

            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExternalProductId).HasMaxLength(256);
            builder.Property(x => x.Sku).HasMaxLength(256);
            builder.Property(x => x.Name).HasMaxLength(300);
            builder.Property(x => x.Thumbnail).HasMaxLength(600);

            builder.HasIndex(x => x.ExternalProductId);
            builder.HasIndex(x => x.Sku);
        }
    }
}
