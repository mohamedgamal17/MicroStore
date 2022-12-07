using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Api.Models;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Dtos;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentRequestController : ControllerBase
    {

        private readonly ILocalMessageBus _localMessageBus;

        public PaymentRequestController(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }



        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(PaymentRequestCreatedDto))]
        public async Task<PaymentRequestCreatedDto> CreatePaymentRequest(CreatePaymentRequestModel model)
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

            var result = await _localMessageBus.Send(command);

            return result;
        }

        [HttpPost]
        [Route("process/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type = typeof(PaymentRequestCreatedDto))]

        public async Task<PaymentProcessResultDto> ProcessPaymentRequest(Guid paymentId,ProcessPaymentRequestModel model)
        {
            var command = new ProcessPaymentRequestCommand
            {
                PaymentId = paymentId,
                PaymentGatewayName = model.PaymentGatewayName,
                ReturnUrl = model.ReturnUrl,
                CancelUrl = model.CancelUrl,
            };


           var result = await _localMessageBus.Send(command);

           return result;
        }

        [HttpPost]
        [Route("complete")]
        public async Task<PaymentRequestCompletedDto> CompletePaymentRequest(CompletePaymentRequestModel model)
        {
            var command = new CompletePaymentRequestCommand
            {
                PaymentGatewayName = model.PaymentGatewayName,
                Token = model.Token,
            };

            var result = await _localMessageBus.Send(command);

            return result;
        }
    }
}
