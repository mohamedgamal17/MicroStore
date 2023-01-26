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

            builder.Navigation(x => x.Role).AutoInclude();
        }
    }
}
