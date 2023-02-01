using Duende.IdentityServer.EntityFramework.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces
{
    public interface IApiResourceRepository : IRepository<ApiResource>
    { 

        Task UpdateApiResourceAsync(ApiResource apiResource, CancellationToken cancellationToken = default);
    }
}
