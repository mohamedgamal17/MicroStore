using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ShipppingSystemEntityTypeConfiguration : IEntityTypeConfiguration<ShippingSystem>
    {
        public void Configure(EntityTypeBuilder<ShippingSystem> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.Name).HasMaxLength(256).IsRequired();

            builder.Property(x => x.DisplayName).HasMaxLength(256).IsRequired();

            builder.Property(x => x.Image).HasMaxLength(500);

            builder.Property(x => x.IsEnabled);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
