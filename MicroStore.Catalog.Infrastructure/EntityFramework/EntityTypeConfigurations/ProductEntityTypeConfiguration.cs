using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(600);

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(600);

            builder.Property(x => x.LongDescription)
                .HasMaxLength(2500);

            builder.Property(x => x.Price);

            builder.Property(x => x.OldPrice);

            builder.Navigation(x => x.ProductCategories)
                .AutoInclude();

            builder.HasMany(x => x.ProductCategories)
                .WithOne();


            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasIndex(x => x.Sku)
                .IsUnique();
        }
    }
}
