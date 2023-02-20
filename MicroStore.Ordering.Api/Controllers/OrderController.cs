using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using System.Net;

namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : MicroStoreApiController
    {
        private readonly IOrderCommandService _orderCommandService;

        private readonly IOrderQueryService _orderQueryService;
        public OrderController(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService)
        {
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
        }



        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(OrderListDto))]
        public async Task<IActionResult> RetirveOrderList([FromQuery]PagingAndSortingParamsQueryString @params ,[FromQuery(Name ="user_id")] string? userId= null)
        {
            var queryParams = new PagingAndSortingQueryParams
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await _orderQueryService.ListAsync(queryParams, userId);

            return FromResultV2(result,HttpStatusCode.OK);
        }

      

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]

        public async Task<IActionResult> RetirveOrder(Guid orderId)
        {

            var result = await _orderQueryService.GetAsync(orderId);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("order_number/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<IActionResult> RetirveOrderByNumber(string orderNumber)
        {

            var result = await _orderQueryService.GetByOrderNumberAsync(orderNumber);

            return FromResultV2(result, HttpStatusCode.OK);
        }


        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]

        public async Task<IActionResult> SubmitOrder([FromBody]CreateOrderModel model)
        {
            var result  = await _orderCommandService.CreateOrderAsync(model);

            return FromResultV2(result,HttpStatusCode.Accepted);
        }



        [HttpPost("fullfill/{orderId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]

        public async Task<IActionResult> FullfillOrder(Guid orderId,[FromBody] FullfillOrderModel model)
        {

            var result = await _orderCommandService.FullfillOrderAsync(orderId,model);

            return FromResultV2(result, HttpStatusCode.Accepted);
        }


        [HttpPost("complete/{orderId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            var result = await _orderCommandService.CompleteOrderAsync(orderId);

            return FromResultV2(result, HttpStatusCode.Accepted);
        }



        [HttpPost("cancel/{orderId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> CancelOrder(Guid orderId, [FromBody] CancelOrderModel model)
        {
            var result = await _orderCommandService.CancelOrderAsync(orderId,model);

            return FromResultV2(result, HttpStatusCode.Accepted);

        }


    }
}
