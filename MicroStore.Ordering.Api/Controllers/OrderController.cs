﻿using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Api.Models;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.Abstractions.Queries;
using MicroStore.Ordering.Application.Abstractions.Security;
using MicroStore.Ordering.Application.Dtos;
namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : MicroStoreApiController
    {

        private readonly IPublishEndpoint _publishEndPoint;

        public OrderController(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        [HttpGet]
        [Route("")]
        [RequiredScope(OrderingScope.Order.List)]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<PagedResult<OrderListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveOrderList([FromQuery]PagingAndSortingQueryParams @params)
        {
            var query = new GetOrderListQuery
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("user/{userId}")]
        [RequiredScope(OrderingScope.Order.List)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<OrderListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveUserOrderList(string userId, [FromQuery]  PagingAndSortingQueryParams @params)
        {
            var query = new GetUserOrderListQuery
            {
                UserId = userId,
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{orderId}")]
        [RequiredScope(OrderingScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<OrderListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveOrder(Guid orderId, [FromQuery] PagingAndSortingQueryParams @params)
        {
            var query = new GetOrderQuery
            {
                OrderId = orderId,
            };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpPost("submit")]
        [RequiredScope(OrderingScope.Order.Submit)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(OrderSubmitedDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> SubmitOrder([FromBody]SubmitOrderModel model)
        {
            var command = new SubmitOrderCommand
            {
                UserId = model.UserId,
                ShippingAddress = model.ShippingAddress,
                BillingAddress = model.BillingAddress,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubTotal,
                TaxCost = model.TaxCost,
                TotalPrice = model.TotalPrice,
                OrderItems = model.OrderItems,
                SubmissionDate = DateTime.UtcNow
            };

            var result = await Send(command);

            return FromResult(result);
        }



        [HttpPost("fullfill/{orderId}")]
        [RequiredScope(OrderingScope.Order.Fullfill)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> FullfillOrder(Guid orderId,[FromBody] FullfillOrderModel model)
        {
            var command = new FullfillOrderCommand
            {
                OrderId = orderId,
                ShipmentId = model.ShipmentId
            };

            var result = await Send(command);

            return FromResult(result);
        }


        [HttpPost("complete/{orderId}")]
        [RequiredScope(OrderingScope.Order.Complete)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            var command = new CompleteOrderCommand
            {
                OrderId = orderId,
                ShipedDate = DateTime.UtcNow
            };

            var result = await Send(command);

            return FromResult(result);
        }



        [HttpPost("cancel/{orderId}")]
        [RequiredScope(OrderingScope.Order.Cancel)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CancelOrder(Guid orderId, [FromBody] CancelOrderModel model)
        {
            var command = new CancelOrderCommand
            {
                OrderId = orderId,
                Reason = model.Reason,
                CancellationDate = DateTime.UtcNow
            };

            var result = await Send(command);

            return FromResult(result);

        }


    }
}
