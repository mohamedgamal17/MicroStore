using Duende.IdentityServer.EntityFramework.Interfaces;

namespace MicroStore.IdentityProvider.OAuth.Application.Common
{
    public interface IApplicationConfigurationDbContext : IConfigurationDbContext
    {
    }

    public interface IApplicationPersistedGrantDbContext : IPersistedGrantDbContext { }
}
