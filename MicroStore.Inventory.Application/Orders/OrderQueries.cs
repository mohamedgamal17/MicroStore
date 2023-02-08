using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Dtos;

namespace MicroStore.Inventory.Application.Orders
{
    public class GetOrderListQuery : PagingQueryParams, IQuery<PagedResult<OrderListDto>>
    {
    }
    public class GetUserOrderListQuery : PagingQueryParams, IQuery<PagedResult<OrderListDto>>
    {
        public string UserId { get; set; }
    }
    public class GetOrderWithExternalIdQuery : IQuery<OrderDto>
    {
        public string ExternalOrderId { get; set; }
    }
    public class GetOrderQuery : IQuery<OrderDto>
    {
        public Guid OrderId { get; set; }
    }
}
