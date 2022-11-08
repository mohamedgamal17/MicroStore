using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.Domain.Entities;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Payment.IntegrationEvents.Responses;

namespace MicroStore.Ordering.Application.Commands
{
    public class PayOrderCommandHandler : CommandHandler<PayOrderCommand, PaymentDto>
    {

        private readonly IRequestClient<CheckOrderRequest> _checkOrderRequestClinet;
        private readonly IRequestClient<CreatePaymentRequest> _createPaymentRequestClinet;
        private readonly IPublishEndpoint _publishEndPoint;

        public PayOrderCommandHandler(IPublishEndpoint publishEndPoint, IRequestClient<CheckOrderRequest> checkOrderRequestClinet, IRequestClient<CreatePaymentRequest> createPaymentRequestClinet)
        {
            _publishEndPoint = publishEndPoint;
            _checkOrderRequestClinet = checkOrderRequestClinet;
            _createPaymentRequestClinet = createPaymentRequestClinet;
        }


        public override async Task<PaymentDto> Handle(PayOrderCommand request, CancellationToken cancellationToken)
        {

            var (orderResponse, orderNotFound) = await _checkOrderRequestClinet
                .GetResponse<OrderResponse, OrderNotFound>(new CheckOrderRequest
                {
                    OrderId = request.OrderId
                });

            if (orderNotFound.IsCompletedSuccessfully)
            {
                throw new EntityNotFoundException(typeof(OrderStateEntity), request.OrderId);
            }

            var response = await orderResponse;

            var paymentResponse = await _createPaymentRequestClinet.GetResponse<PaymentCreatedResponse>(new CreatePaymentRequest
            {
                OrderId = response.Message.OrderId,
                OrderNumber = response.Message.OrderNumber,
                TotalPrice = response.Message.Total
            });

            await _publishEndPoint.Publish(new OrderOpenedEvent
            {
                OrderId = request.OrderId,
                TransactionId = paymentResponse.Message.TransactionId
            });



            return new PaymentDto
            {
                TransactionId = paymentResponse.Message.TransactionId,
                Gateway = paymentResponse.Message.Gateway
            };

        }



    }
}
