using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public interface IApiScopeCommandService : IApplicationService
    {
        Task<ResultV2<ApiScopeDto>> CreateAsync(ApiScopeModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<ApiScopeDto>> UpdateAsync(int apiScopeId, ApiScopeModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<Unit>> DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default);
    }
}
