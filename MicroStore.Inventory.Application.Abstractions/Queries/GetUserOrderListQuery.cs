using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetUserOrderListQuery : PagingQueryParams , IQuery<PagedResult<OrderListDto>>
    {
        public string UserId { get; set; }
    }
}
