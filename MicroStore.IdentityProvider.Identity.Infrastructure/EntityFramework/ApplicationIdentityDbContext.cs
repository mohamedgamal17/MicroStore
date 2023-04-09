using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;
using MicroStore.IdentityProvider.Identity.Infrastructure.Consts;
using Volo.Abp.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application.Common;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework
{
    public class ApplicationIdentityDbContext : DbContext, IApplicationIdentityDbContext, ITransientDependency
    {
        public DbSet<ApplicationIdentityUser> Users { get; set; } = default!;

        public DbSet<ApplicationIdentityRole> Roles { get; set; } = default!;


        private StoreOptions? GetStoreOptions() => this.GetService<IDbContextOptions>()
            .Extensions.OfType<CoreOptionsExtension>()
            .FirstOrDefault()?.ApplicationServiceProvider
            ?.GetService<IOptions<IdentityOptions>>()
            ?.Value?.Stores;

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        private sealed class PersonalDataConverter : ValueConverter<string, string>
        {
            public PersonalDataConverter(IPersonalDataProtector protector) : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
            { }
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var storeOptions = GetStoreOptions();
            var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
            PersonalDataConverter? converter = null;

            builder.HasDefaultSchema(IdentityDbContextConsts.DbSchema);

            builder.Entity<ApplicationIdentityUser>(b =>
            {


                if (encryptPersonalData)
                {
                    converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                    var personalDataProps = typeof(ApplicationIdentityUser).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in personalDataProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException("[ProtectedPersonalData] only works strings by default.");
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }


            });

            builder.Entity<ApplicationIdentityUserToken>(b =>
            {

                if (encryptPersonalData)
                {
                    var tokenProps = typeof(ApplicationIdentityUserToken).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in tokenProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException("[ProtectedPersonalData] only works strings by default.");
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }
            });


        }




    }
}
