using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.Domain.Security;
namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/user/orders")]
    public class UserOrderController : MicroStoreApiController
    {


        private readonly IOrderQueryService _orderQueryService;

        public UserOrderController(IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        [HttpGet]
        [Route("")]
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<OrderListDto>))]
        public async Task<IActionResult> RetirveUserOrderList([FromQuery] PagingQueryParams queryParam)
        {
            var result = await _orderQueryService.ListOrderAsync(queryParam, CurrentUser.Id.ToString()!);

            return result.ToOk();
        }


        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<IActionResult> RetirveOrderWithExternalId(string orderNumber)
        {
            var result = await _orderQueryService.GetOrderByNumberAsync(orderNumber);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{orderId}")]
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]

        public async Task<IActionResult> RetriveOrder(string orderId)
        {
            var result = await _orderQueryService.GetOrderAsync(orderId);

            return result.ToOk(); 
        }
    }
}
