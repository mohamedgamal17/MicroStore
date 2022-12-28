using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Api.Models;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentRequestController : MicroStoreApiController
    {

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<PaymentRequestCreatedDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CreatePaymentRequest(CreatePaymentRequestModel model)
        {
            var command = new CreatePaymentRequestCommand
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNubmer,
                UserId = model.UserId,
                TaxCost = model.TaxCost,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubtTotal,
                TotalCost = model.TotalCost,
                Items = model.Items,
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("process/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(PaymentProcessResultDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<PaymentProcessResultDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]

        public async Task<IActionResult> ProcessPaymentRequest(Guid paymentId, ProcessPaymentRequestModel model)
        {
            var command = new ProcessPaymentRequestCommand
            {
                PaymentId = paymentId,
                PaymentGatewayName = model.PaymentGatewayName,
                ReturnUrl = model.ReturnUrl,
                CancelUrl = model.CancelUrl,
            };


            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("complete")]

        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(PaymentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<PaymentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CompletePaymentRequest(CompletePaymentRequestModel model)
        {
            var command = new CompletePaymentRequestCommand
            {
                PaymentGatewayName = model.PaymentGatewayName,
                Token = model.Token,
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
