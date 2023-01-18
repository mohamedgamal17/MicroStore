using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetOrderListQuery : PagingQueryParams , IQuery<PagedResult<OrderListDto>>
    {
    }
}
