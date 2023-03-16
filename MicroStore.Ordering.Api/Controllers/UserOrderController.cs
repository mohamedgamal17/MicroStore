using Microsoft.AspNetCore.Authorization;
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
    [ApiController]
    [Authorize]
    [Route("api/user/orders")]
    public class UserOrderController : MicroStoreApiController
    {
        private readonly IOrderCommandService _orderCommandService;

        private readonly IOrderQueryService _orderQueryService;
        private readonly ILogger<UserOrderController> _logger;

        public UserOrderController(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService, ILogger<UserOrderController> logger)
        {
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
  //      [RequiredScope(OrderingScope.Order.Submit)]

        public async Task<IActionResult> SubmitOrder(OrderModel model)
        {

            var orderModel = new CreateOrderModel
            {
                TaxCost = model.TaxCost,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubTotal,
                TotalPrice = model.TotalPrice,
                ShippingAddress = model.ShippingAddress,
                BillingAddress = model.BillingAddress,
                OrderItems = model.OrderItems,
                UserId = CurrentUser.Id.ToString()!,
            };

            var result = await _orderCommandService.CreateOrderAsync(orderModel);

            
            return FromResult(result, HttpStatusCode.Accepted);
        }


        [HttpGet]
        [Route("")]
    //    [RequiredScope(OrderingScope.Order.List)]
        public async Task<IActionResult> RetirveUserOrderList([FromQuery] PagingAndSortingParamsQueryString @params)
        {

            var queryParams = new PagingAndSortingQueryParams
            {

                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await _orderQueryService.ListAsync(queryParams, CurrentUser.Id.ToString()!);

            return FromResult(result, HttpStatusCode.OK);
        }


        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        //      [RequiredScope(OrderingScope.Order.Read)]
        public async Task<IActionResult> RetirveOrder(Guid orderId)
        {

            var result = await _orderQueryService.GetAsync(orderId);

            return FromResult(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("order_number/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        //      [RequiredScope(OrderingScope.Order.Read)]
        public async Task<IActionResult> RetirveOrderByNumber(string orderNumber)
        {

            var result = await _orderQueryService.GetByOrderNumberAsync(orderNumber);

            return FromResult(result, HttpStatusCode.OK);
        }
    }
}
