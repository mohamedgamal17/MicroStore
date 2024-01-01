using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public interface IApiScopeQueryService : IApplicationService
    {
        Task<Result<List<ApiScopeDto>>> ListAsync(ApiScopeListQueryModel queryParams,CancellationToken cancellationToken = default);
        Task<Result<ApiScopeDto>> GetAsync(int apiScopeId, CancellationToken cancellationToken = default);
        Task<Result<List<ApiScopePropertyDto>>> ListProperties(int apiScopeId, CancellationToken cancellationToken = default);
        Task<Result<ApiScopePropertyDto>> GetProperty(int apiScopeId, int propertyId ,CancellationToken cancellationToken = default);
    }
}
