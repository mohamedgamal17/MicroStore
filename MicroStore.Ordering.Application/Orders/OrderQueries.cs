using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Dtos;

namespace MicroStore.Ordering.Application.Orders
{
    public class GetOrderListQuery : PagingAndSortingQueryParams, IQuery<PagedResult<OrderListDto>>
    {
    }

    public class GetOrderQuery : IQuery<OrderDto>
    {
        public Guid OrderId { get; set; }
    }

    public class GetUserOrderListQuery : PagingAndSortingQueryParams, IQuery<PagedResult<OrderListDto>>
    {
        public string UserId { get; set; }
    }
}
