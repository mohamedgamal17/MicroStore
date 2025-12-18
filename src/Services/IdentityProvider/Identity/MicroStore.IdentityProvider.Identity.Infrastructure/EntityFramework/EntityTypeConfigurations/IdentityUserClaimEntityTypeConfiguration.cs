using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class IdentityUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationIdentityUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityUserClaim> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ClaimType).HasMaxLength(256);
            builder.Property(x => x.ClaimValue).HasMaxLength(500);
            builder.HasIndex(x => x.ClaimType);
            builder.Property(x => x.UserId).HasMaxLength(256);
            builder.HasIndex(x => x.UserId);
            builder.ToTable("IdentityUserClaims");
        }
    }
}
