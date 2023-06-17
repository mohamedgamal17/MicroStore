using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductSpecificationAttributeEntityTypeConfiguration : IEntityTypeConfiguration<ProductSpecificationAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductSpecificationAttribute> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x=>x.AttributeId).HasMaxLength(256);

            builder.Property(x=>x.OptionId).HasMaxLength(256);

            builder.HasOne(x => x.Attribute).WithMany().HasForeignKey(x => x.AttributeId);

            builder.HasOne(x => x.Option).WithMany().HasForeignKey(x => x.OptionId);

            builder.Navigation(x => x.Attribute).AutoInclude();

            builder.Navigation(x => x.Option).AutoInclude();

        }
    }
}
