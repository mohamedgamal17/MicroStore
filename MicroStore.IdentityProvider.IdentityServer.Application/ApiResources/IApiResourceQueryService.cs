using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public interface IApiResourceQueryService : IApplicationService
    {
        Task<ResultV2<PagedResult<ApiResourceDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);
        Task<ResultV2<ApiResourceDto>> GetAsync(int apiResourceId , CancellationToken cancellationToken = default);
    }
}
