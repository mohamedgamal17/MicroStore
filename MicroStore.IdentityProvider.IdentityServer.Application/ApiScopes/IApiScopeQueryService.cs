using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using Volo.Abp.Application.Services;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public interface IApiScopeQueryService : IApplicationService
    {
        Task<ResultV2<PagedResult<ApiScopeDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);

        Task<ResultV2<ApiScopeDto>> GetAsync(int apiScopeId, CancellationToken cancellationToken = default);
    }
}
