using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.IdentityProvider.Identity.Application.Domain;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class IdentityUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationIdentityUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityUserRole> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId });
            builder.Property(x => x.UserId).HasMaxLength(256);
            builder.Property(x => x.RoleId).HasMaxLength(256);
            builder.Navigation(x => x.Role).AutoInclude();
        }
    }
}
