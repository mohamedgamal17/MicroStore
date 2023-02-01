using Duende.IdentityServer.EntityFramework.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces
{
    public interface IApiScopeRepository : IRepository<ApiScope>
    {
        Task UpdateApiScopeAsync(ApiScope apiScope, CancellationToken cancellationToken = default);
    }
}
