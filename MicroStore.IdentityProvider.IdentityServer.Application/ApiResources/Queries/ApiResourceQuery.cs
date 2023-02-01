using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Queries
{
    public class GetApiResourceListQuery : PagingQueryParams ,IQuery<PagedResult<ApiResourceDto>>
    {

    }
    public class GetApiResourceQuery : IQuery<ApiResourceDto>
    {
        public int ApiResourceId { get; set; }

    }
}
