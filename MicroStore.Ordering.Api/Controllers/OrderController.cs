using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Api.Models;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Orders;

namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : MicroStoreApiController
    {

        private readonly IPublishEndpoint _publishEndPoint;

        public OrderController(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<PagedResult<OrderListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveOrderList([FromQuery]PagingAndSortingParamsQueryString @params)
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
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<OrderListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveOrder(Guid orderId, [FromQuery] PagingAndSortingParamsQueryString @params)
        {
            var query = new GetOrderQuery
            {
                OrderId = orderId,
            };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(OrderSubmitedDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> SubmitOrder([FromBody]CreateOrderModel model)
        {
            var command = ObjectMapper.Map<CreateOrderModel, SubmitOrderCommand>(model);

            var result = await Send(command);

            return FromResult(result);
        }



        [HttpPost("fullfill/{orderId}")]
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
