using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class SettingsEntityTypeConfiguration : IEntityTypeConfiguration<SettingsEntity>
    {
        public void Configure(EntityTypeBuilder<SettingsEntity> builder)
        {
            builder.Property(x => x.ProviderKey).HasMaxLength(256);

            builder.Property(x => x.Data);

            builder.HasIndex(x => x.ProviderKey).IsUnique();
        }
    }
}
