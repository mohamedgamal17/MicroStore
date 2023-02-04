using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes
{
    public class GetApiScopeListQuery : PagingQueryParams, IQuery<PagedResult<ApiScopeDto>>
    {

    }


    public class GetApiScopeQuery : IQuery<ApiScopeDto>
    {
        public int ApiScopeId { get; set; }
    }
}
