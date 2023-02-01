using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients.Queries
{
    public class GetClientListQuery : PagingQueryParams , IQuery<PagedResult<ClientDto>> { }

    public class GetClientQuery : IQuery<ClientDto> 
    {
        public int ClientId { get; set; }

    }

}
