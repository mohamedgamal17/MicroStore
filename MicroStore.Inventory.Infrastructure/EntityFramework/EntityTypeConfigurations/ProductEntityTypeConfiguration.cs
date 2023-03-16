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

            builder.Property(x=> x.Sku).HasMaxLength(256);

            builder.Property(x => x.Thumbnail).HasMaxLength(500).IsRequired(false).HasDefaultValue(string.Empty);

            builder.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(300);

            builder.Property(x => x.Stock);

            builder.Property(x => x.AllocatedStock);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasIndex(x => x.Sku).IsUnique();
        }
    }
}
