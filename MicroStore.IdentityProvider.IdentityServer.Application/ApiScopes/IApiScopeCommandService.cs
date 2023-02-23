using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public interface IApiScopeCommandService : IApplicationService
    {
        Task<UnitResultV2<ApiScopeDto>> CreateAsync(ApiScopeModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2<ApiScopeDto>> UpdateAsync(int apiScopeId, ApiScopeModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2> DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default);
    }
}
