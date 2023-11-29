using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductTagEntityTypeConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(256);

            builder.Property(x => x.Description).HasMaxLength(650).IsRequired(false);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
