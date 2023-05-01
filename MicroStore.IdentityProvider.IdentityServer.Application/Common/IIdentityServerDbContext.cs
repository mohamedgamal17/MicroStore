using Duende.IdentityServer.EntityFramework.Interfaces;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Common
{
    public interface IApplicationConfigurationDbContext : IConfigurationDbContext 
    {
    }

    public interface IApplicationPersistedGrantDbContext : IPersistedGrantDbContext { }
}
