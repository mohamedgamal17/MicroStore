using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroStore.IdentityProvider.Identity.Application.Domain;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class IdentityUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationIdentityUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityUser> builder)
        {
            builder.ToTable("IdentityUsers");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(256);
            builder.HasIndex(x => x.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
            builder.HasIndex(x => x.NormalizedEmail).HasDatabaseName("EmailIndex");
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
            builder.Property(x => x.UserName).HasMaxLength(256);
            builder.Property(x => x.GivenName).HasMaxLength(256).IsRequired(false).HasDefaultValue(string.Empty);
            builder.Property(x => x.FamilyName).HasMaxLength(256).IsRequired(false).HasDefaultValue(string.Empty); 
            builder.Property(x => x.NormalizedUserName).HasMaxLength(256);
            builder.Property(x => x.Email).HasMaxLength(256);
            builder.Property(x => x.PhoneNumber).HasMaxLength(30);
            builder.Property(x => x.NormalizedEmail).HasMaxLength(256);
            builder.Property(x => x.ConcurrencyStamp).HasMaxLength(256);
            builder.Property(x => x.SecurityStamp).HasMaxLength(256);
            builder.Property(x => x.PasswordHash).HasMaxLength(500);
            builder.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.UserId).IsRequired();
            builder.HasMany(x => x.UserLogins).WithOne().HasForeignKey(x => x.UserId).IsRequired();
            builder.HasMany(x => x.UserTokens).WithOne().HasForeignKey(x => x.UserId).IsRequired();
            builder.HasMany(x => x.UserRoles).WithOne().HasForeignKey(x => x.UserId).IsRequired();
           
            builder.Navigation(x => x.UserRoles).AutoInclude(true);
            builder.Navigation(x => x.UserClaims).AutoInclude(true);
        }
    }
}
