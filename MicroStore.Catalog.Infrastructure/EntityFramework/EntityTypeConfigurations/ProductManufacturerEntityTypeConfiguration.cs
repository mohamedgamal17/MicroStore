using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductManufacturerEntityTypeConfiguration : IEntityTypeConfiguration<ProductManufacturer>
    {
        public void Configure(EntityTypeBuilder<ProductManufacturer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.ProductId).HasMaxLength(256);

            builder.Property(x => x.ManufacturerId).HasMaxLength(256);

            builder.HasOne(x => x.Manufacturer).WithMany().HasForeignKey(x => x.ManufacturerId);

            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.ManufacturerId);
        }
    }
}
