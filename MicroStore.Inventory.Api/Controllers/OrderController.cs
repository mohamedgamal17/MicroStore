using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Orders;
using System.Net;

namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/inventory/orders")]
    [Authorize]
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

        public async Task<IActionResult> RetirveOrderList([FromQuery] PagingParamsQueryString @params , [FromQuery(Name ="user_id")] string? userId=  null)
        {
            var queryParams = new PagingQueryParams
            {
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await _orderQueryService.ListOrderAsync(queryParams, userId);

            return FromResultV2(result, HttpStatusCode.OK);
        }



        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]

        public async Task<IActionResult> RetirveOrderWithExternalId(string orderNumber)
        {

            var result = await _orderQueryService.GetOrderByNumberAsync(orderNumber);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        
        public async Task<IActionResult> RetriveOrder(string orderId)
        {

            var result = await _orderQueryService.GetOrderAsync(orderId);

            return FromResultV2(result, HttpStatusCode.OK);
        }
    }
}
