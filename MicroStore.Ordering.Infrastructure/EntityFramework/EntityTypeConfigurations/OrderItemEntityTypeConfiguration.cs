using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MicroStore.Ordering.Application.StateMachines;
namespace MicroStore.Ordering.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {

            builder.HasKey(x => x.Id);
            builder.Property(x => x.ItemName).HasMaxLength(500);
            builder.Property(x => x.ProductImage).HasMaxLength(600);
            builder.HasIndex(x => x.ProductId);
        }
    }
}
