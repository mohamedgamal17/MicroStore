using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductImageEntityTypeConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.ProductId).HasMaxLength(256);

            builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(600);

            builder.Property(x => x.DisplayOrder).IsRequired();

            builder.HasOne<Product>().WithMany(x => x.ProductImages)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
