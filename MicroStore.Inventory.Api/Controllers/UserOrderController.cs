﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.Domain.Security;
using System.Net;
namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/user/orders")]
    public class UserOrderController : MicroStoreApiController
    {
        private readonly IApplicationCurrentUser _applicationCurrentUser;

        private readonly IOrderQueryService _orderQueryService;

        public UserOrderController(IApplicationCurrentUser applicationCurrentUser, IOrderQueryService orderQueryService)
        {
            _applicationCurrentUser = applicationCurrentUser;
            _orderQueryService = orderQueryService;
        }

        [HttpGet]
        [Route("")]
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<OrderListDto>))]
        public async Task<IActionResult> RetirveUserOrderList([FromQuery] PagingParamsQueryString @params)
        {
            var queryParam = new PagingQueryParams
            {
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await _orderQueryService.ListOrderAsync(queryParam, _applicationCurrentUser.Id);

            return FromResult(result, HttpStatusCode.OK);
        }


        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<IActionResult> RetirveOrderWithExternalId(string orderNumber)
        {
            var result = await _orderQueryService.GetOrderByNumberAsync(orderNumber);

            return FromResult(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{orderId}")]
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]

        public async Task<IActionResult> RetriveOrder(string orderId)
        {
            var result = await _orderQueryService.GetOrderAsync(orderId);

            return FromResult(result, HttpStatusCode.OK);
        }
    }
}
