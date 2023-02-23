using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public interface IApiScopeCommandService : IApplicationService
    {
        Task<UnitResult<ApiScopeDto>> CreateAsync(ApiScopeModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<ApiScopeDto>> UpdateAsync(int apiScopeId, ApiScopeModel model, CancellationToken cancellationToken = default);

        Task<UnitResult> DeleteAsync(int apiScopeId, CancellationToken cancellationToken = default);
    }
}
