using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Abstractions.Dtos;

namespace MicroStore.Ordering.Application.Abstractions.Queries
{
    public class GetOrderListQuery : PagingAndSortingQueryParams , IQuery<PagedResult<OrderListDto>>
    {
    }
}
