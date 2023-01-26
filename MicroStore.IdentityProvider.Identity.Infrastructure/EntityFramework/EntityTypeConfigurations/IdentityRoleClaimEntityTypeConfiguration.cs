using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.IdentityProvider.Identity.Application.Domain;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class IdentityRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationIdentityRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityRoleClaim> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ClaimType).HasMaxLength(256);
            builder.Property(x => x.ClaimValue).HasMaxLength(500);
            builder.HasIndex(x => x.ClaimType);
            builder.ToTable("IdentityRoleClaims");
        }
    }
}
