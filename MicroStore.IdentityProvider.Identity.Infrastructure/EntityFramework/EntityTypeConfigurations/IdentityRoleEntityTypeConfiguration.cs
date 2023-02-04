using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.IdentityProvider.Identity.Application.Domain;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class IdentityRoleEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationIdentityRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityRole> builder)
        {

            builder.HasKey(r => r.Id);
            builder.Property(x => x.Id).HasMaxLength(256);
            builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
            builder.ToTable("AspNetRoles");
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            builder.Property(u => u.Name).HasMaxLength(256);

            builder.Property(x => x.Description).HasDefaultValue(string.Empty).HasMaxLength(500);
            builder.Property(u => u.NormalizedName).HasMaxLength(256);
            builder.Property(x => x.ConcurrencyStamp).HasMaxLength(256);


            builder.HasMany(x => x.RoleClaims).WithOne().HasForeignKey(x => x.RoleId).IsRequired();

            builder.Navigation(x => x.RoleClaims).AutoInclude();
            
        }
    }
}
