using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class IdentityUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationIdentityUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityUserToken> builder)
        {
            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            builder.ToTable("IdentityUserTokens");
            builder.Property(x => x.UserId).HasMaxLength(256);
            builder.Property(x => x.LoginProvider).HasMaxLength(256);
            builder.Property(x => x.Name).HasMaxLength(256);

            builder.Property(x => x.Value).HasMaxLength(500);
        }
    }
}
