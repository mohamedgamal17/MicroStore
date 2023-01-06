using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Consts;
using MicroStore.Ordering.Events;
using MicroStore.BuildingBlocks.Results;
using System.Net;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Application.Abstractions.Abstractions.Common;

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

        public override async Task<ResponseResult> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrder(request.OrderId);

            if (order == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Order entity with id {request.OrderId} is not exist"
                });
            }
            if (order.CurrentState != OrderStatusConst.Fullfilled)
            {
                var errorInfo = new ErrorInfo {
                    Message = $"invalid order status. " + $"please make sure that order is in {OrderStatusConst.Fullfilled} status to be able to complete the order"
                };

                return ResponseResult.Failure((int)(HttpStatusCode.BadRequest), errorInfo);
            }

            var orderCompletedEvent = new OrderCompletedEvent
            {
                OrderId = request.OrderId,
                ShippedDate = request.ShipedDate
            };

            await _publishEndpoint.Publish(orderCompletedEvent, cancellationToken);

            return ResponseResult.Success((int)(HttpStatusCode.Accepted));
        }
    }
}
