using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Shipping.Application.Abstraction.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetUserShipmentListQuery : PagingQueryParams , IQuery<PagedResult<ShipmentListDto>>
    {
        public string UserId { get; set; }
    }
}
