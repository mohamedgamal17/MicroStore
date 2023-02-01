using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework
{
    [ExposeServices(typeof(IApplicationConfigurationDbContext))]
    public class ApplicationConfigurationDbContext : ConfigurationDbContext<ApplicationConfigurationDbContext>, IApplicationConfigurationDbContext , IScopedDependency
    {
        public ApplicationConfigurationDbContext(DbContextOptions<ApplicationConfigurationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntities(modelBuilder);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            var apiResourceBuilder = modelBuilder.Entity<ApiResource>();

            apiResourceBuilder.Navigation(x => x.Properties).AutoInclude(true);
            apiResourceBuilder.Navigation(x => x.Secrets).AutoInclude(true);
            apiResourceBuilder.Navigation(x => x.UserClaims).AutoInclude(true);
            apiResourceBuilder.Navigation(x => x.Scopes).AutoInclude(true);

            var apiScopeBuilder = modelBuilder.Entity<ApiScope>();
            apiResourceBuilder.Navigation(x => x.Properties).AutoInclude(true);
            apiResourceBuilder.Navigation(x => x.UserClaims).AutoInclude(true);
            apiResourceBuilder.Navigation(x => x.UserClaims).AutoInclude(true);

            var clientBuilder = modelBuilder.Entity<Client>();
            clientBuilder.Navigation(x => x.AllowedGrantTypes).AutoInclude(true);
            clientBuilder.Navigation(x => x.RedirectUris).AutoInclude(true);
            clientBuilder.Navigation(x => x.PostLogoutRedirectUris).AutoInclude(true);
            clientBuilder.Navigation(x => x.Claims).AutoInclude(true);
            clientBuilder.Navigation(x => x.ClientSecrets).AutoInclude(true);
            clientBuilder.Navigation(x => x.Properties).AutoInclude(true);

        }
    }

    [ExposeServices(typeof(IApplicationPersistedGrantDbContext))]
    public class ApplicationPersistedGrantDbContext : PersistedGrantDbContext<ApplicationPersistedGrantDbContext>, IApplicationPersistedGrantDbContext , IScopedDependency
    {
        public ApplicationPersistedGrantDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
