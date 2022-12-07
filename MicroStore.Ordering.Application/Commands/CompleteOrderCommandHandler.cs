using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Consts;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.Events.Responses;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using MicroStore.Ordering.Application.Abstractions.Interfaces;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Commands
{
    public class CompleteOrderCommandHandler : CommandHandler<CompleteOrderCommand>
    {

        private readonly IPublishEndpoint _publishEndpoint;

        private readonly IOrderRepository _orderRepository;
        public CompleteOrderCommandHandler(IPublishEndpoint publishEndpoint, IOrderRepository orderRepository)
        {
            _publishEndpoint = publishEndpoint;
            _orderRepository = orderRepository;
        }

        public override async Task<Unit> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrder(request.OrderId);

            if(order == null)
            {
                throw new EntityNotFoundException(typeof(OrderStateEntity), request.OrderId);
            }

            if (order.CurrentState != OrderStatusConst.Fullfilled)
            {
                throw new UserFriendlyException($"invalid order status. " +
                    $"please make sure that order is in {OrderStatusConst.Fullfilled} status to be able to complete the order");
            }

            CompleteOrderIntegrationEvent orderCompletedEvent = new CompleteOrderIntegrationEvent
            {
                OrderId = request.OrderId,
                ShippedDate = request.ShipedDate
            };

            await _publishEndpoint.Publish(orderCompletedEvent, cancellationToken);

            return Unit.Value;
        }
    }
}
