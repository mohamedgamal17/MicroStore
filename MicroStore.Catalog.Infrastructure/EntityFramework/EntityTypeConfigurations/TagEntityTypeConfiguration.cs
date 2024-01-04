using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(256);

            builder.Property(x => x.Description).HasMaxLength(650).IsRequired(false);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
