using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Api.Models;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Dtos;

namespace MicroStore.Ordering.Api.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : MicroStoreApiController
    {

        private readonly IPublishEndpoint _publishEndPoint;

        public OrderController(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        
        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status102Processing, Type = typeof(OrderSubmitedDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status102Processing, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [ProducesResponseType(StatusCodes.Status102Processing, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status102Processing, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelOrder(Guid orderId, CancelOrderModel model)
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
