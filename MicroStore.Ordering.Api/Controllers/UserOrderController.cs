using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using MicroStore.Ordering.Application.Security;
using System.Net;

namespace MicroStore.Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/user/orders")]
    public class UserOrderController : MicroStoreApiController
    {

        private readonly IApplicationCurrentUser _currentUser;

        private readonly IOrderCommandService _orderCommandService;

        private readonly IOrderQueryService _orderQueryService;

        public UserOrderController(IApplicationCurrentUser currentUser, IOrderCommandService orderCommandService, IOrderQueryService orderQueryService)
        {
            _currentUser = currentUser;
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
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
                UserId = _currentUser.Id,
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

            var result = await _orderQueryService.ListAsync(queryParams, _currentUser.Id);

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
