using Duende.IdentityServer.EntityFramework.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Common
{
    public interface IApiResourceRepository : IRepository<ApiResource>
    {

        Task UpdateApiResourceAsync(ApiResource apiResource, CancellationToken cancellationToken = default);
    }
}
