using MassTransit;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.Events;
using Volo.Abp.Domain.Entities;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Ordering.Application.Abstractions.Commands;

namespace MicroStore.Ordering.Application.Commands
{
    public class ConfirmOrderCommandHandler : CommandHandler<ConfirmOrderCommand>
    {
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly IRequestClient<CheckOrderRequest> _requestClinet;

        public ConfirmOrderCommandHandler(IPublishEndpoint publishEndPoint, IRequestClient<CheckOrderRequest> requestClinet)
        {
            _publishEndPoint = publishEndPoint;
            _requestClinet = requestClinet;
        }


        public override async Task<Unit> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var (order, orderNotFound) = await _requestClinet.GetResponse<OrderResponse, OrderNotFound>(new CheckOrderRequest
            {
                OrderId = request.OrderId
            });

            if (orderNotFound.IsCompletedSuccessfully)
            {
                throw new EntityNotFoundException(typeof(OrderStateEntity), request.OrderId);
            }

            var response = await order;

            await _publishEndPoint.Publish(new OrderConfirmedEvent
            {
                OrderId = response.Message.OrderId,
                ConfirmationDate = DateTime.UtcNow
            });

            return Unit.Value;
        }
    }
}
