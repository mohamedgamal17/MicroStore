using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.IdentityProvider.Identity.Application.Domain;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class IdentityUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationIdentityUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityUserLogin> builder)
        {
            builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
            builder.Property(x => x.LoginProvider).HasMaxLength(256);
            builder.Property(x => x.ProviderKey).HasMaxLength(256);
            builder.Property(x => x.ProviderDisplayName).HasMaxLength(256);
            builder.ToTable("IdentityUserLogins");
        }
    }
}
