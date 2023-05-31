using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.Payment.Application.Domain;
namespace MicroStore.Payment.Application.EntityFramework.EntityTypeConfigurations
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
