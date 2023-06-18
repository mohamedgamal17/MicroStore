using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class SpecificationAttributeEntityTypeConfiguration : IEntityTypeConfiguration<SpecificationAttribute>
    {
        public void Configure(EntityTypeBuilder<SpecificationAttribute> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x => x.Name).HasMaxLength(256);

            builder.Property(x => x.Description).HasMaxLength(650);

            builder.HasIndex(x => x.Name);

            builder.HasMany(x => x.Options).WithOne().HasForeignKey(x=> x.SpecificationAttributeId);
        }
    }

    public class SpecificationAttributeOptionEntityTypeConfiguration : IEntityTypeConfiguration<SpecificationAttributeOption>
    {
        public void Configure(EntityTypeBuilder<SpecificationAttributeOption> builder)
        {
            builder.Property(x => x.Id).HasMaxLength(256);

            builder.Property(x=> x.Name).HasMaxLength(256);
        }
    }


}
