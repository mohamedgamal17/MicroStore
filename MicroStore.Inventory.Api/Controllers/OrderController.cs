using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.Domain.Security;
using System.Net;

namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/inventory/orders")]
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
    public class OrderController : MicroStoreApiController
    {


        private readonly IOrderQueryService _orderQueryService;

        public OrderController( IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PagedResult<OrderListDto>))]

        public async Task<IActionResult> RetirveOrderList([FromQuery] PagingQueryParams queryParams, [FromQuery(Name ="user_id")] string? userId=  null)
        {
            var result = await _orderQueryService.ListOrderAsync(queryParams, userId);

            return result.ToOk();
        }



        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]

        public async Task<IActionResult> RetirveOrderWithExternalId(string orderNumber)
        {

            var result = await _orderQueryService.GetOrderByNumberAsync(orderNumber);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        
        public async Task<IActionResult> RetriveOrder(string orderId)
        {

            var result = await _orderQueryService.GetOrderAsync(orderId);

            return result.ToOk();
        }
    }
}
