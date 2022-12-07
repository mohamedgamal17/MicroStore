using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Consts;
using MicroStore.Ordering.Application.Abstractions.Interfaces;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.IntegrationEvents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Ordering.Application.Commands
{
    public class FullfillOrderCommandHandler : CommandHandler<FullfillOrderCommand>
    {

        private readonly IPublishEndpoint _publishEndPoint;

        private readonly IOrderRepository _orderRepository;

        public FullfillOrderCommandHandler(IPublishEndpoint publishEndPoint, IOrderRepository orderRepository)
        {
            _publishEndPoint = publishEndPoint;
            _orderRepository = orderRepository;
        }

        public override async Task<Unit> Handle(FullfillOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrder(request.OrderId);

            if (order == null)
            {
                throw new EntityNotFoundException(typeof(OrderStateEntity), request.OrderId);
            }

            if(order.CurrentState != OrderStatusConst.Approved)
            {
                throw new UserFriendlyException($"invalid order status. " +
                    $"please make sure that order is in {OrderStatusConst.Approved} status to be able to fullfill the order");
            }

            FullfillOrderIntegrationEvent orderFulfillmentCompletedEvent = new FullfillOrderIntegrationEvent
            {
                OrderId = request.OrderId,
                ShipmentId = request.ShipmentId,
                ShipmentSystem = request.ShipmentSystem,
            };

            await _publishEndPoint.Publish(orderFulfillmentCompletedEvent);

            return Unit.Value;
        }
    }
}
