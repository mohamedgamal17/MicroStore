using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;


namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsFeaturedProduct).IsRequired();

            builder.Navigation(x => x.Category).AutoInclude();

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
