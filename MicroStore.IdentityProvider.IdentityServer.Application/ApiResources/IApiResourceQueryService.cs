using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources
{
    public interface IApiResourceQueryService : IApplicationService
    {
        Task<Result<PagedResult<ApiResourceDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);
        Task<Result<ApiResourceDto>> GetAsync(int apiResourceId , CancellationToken cancellationToken = default);
    }
}
