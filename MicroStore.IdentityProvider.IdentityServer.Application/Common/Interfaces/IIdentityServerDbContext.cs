using Duende.IdentityServer.EntityFramework.Interfaces;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces
{
    public interface IIdentityServerDbContext : IConfigurationDbContext , IPersistedGrantDbContext
    {
    }

    public interface IApplicationConfigurationDbContext : IConfigurationDbContext { }

    public interface IApplicationPersistedGrantDbContext : IPersistedGrantDbContext { }
}
