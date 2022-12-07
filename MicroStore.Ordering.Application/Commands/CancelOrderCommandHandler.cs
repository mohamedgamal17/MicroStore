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
    public class CancelOrderCommandHandler : CommandHandler<CancelOrderCommand>
    {
        private readonly IPublishEndpoint _publishEndPoint;

        private readonly IOrderRepository _orderRepository;
        public CancelOrderCommandHandler(IPublishEndpoint publishEndPoint, IOrderRepository orderRepository)
        {
            _publishEndPoint = publishEndPoint;
            _orderRepository = orderRepository;
        }

        public override async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {

            var order = await _orderRepository.GetOrder(request.OrderId);

            if(order == null)
            {
                throw new EntityNotFoundException(typeof(OrderStateEntity), request.OrderId);
            }

            if(order.CurrentState == OrderStatusConst.Cancelled)
            {
                throw new UserFriendlyException("order state is already canceled");
            }

            CancelOrderIntegrationEvent orderCancelledEvent = new CancelOrderIntegrationEvent
            {
                OrderId = request.OrderId,
                Reason = request.Reason,
                CancellationDate = request.CancellationDate,
            };

            await _publishEndPoint.Publish(orderCancelledEvent, cancellationToken);

            return Unit.Value;
        }
    }
}
