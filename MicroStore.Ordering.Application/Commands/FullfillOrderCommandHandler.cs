﻿using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Common;
using MicroStore.Ordering.Application.Abstractions.Consts;
using MicroStore.Ordering.Events;
using System.Net;
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

        public override async Task<ResponseResult<Unit>> Handle(FullfillOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrder(request.OrderId);

            if (order == null)
            {
                return Failure(HttpStatusCode.NotFound, 
                    new ErrorInfo { Message = $"Order entity with id {request.OrderId} is not exist" });
            }

            if (order.CurrentState != OrderStatusConst.Approved)
            {
                var errorInfo = new ErrorInfo
                {
                    Message = $"invalid order status. " +
                    $"please make sure that order is in {OrderStatusConst.Approved} status to be able to fullfill the order"
                };

                return Failure(HttpStatusCode.BadRequest, errorInfo);
            }

            var orderFulfillmentCompletedEvent = new OrderFulfillmentCompletedEvent
            {
                OrderId = request.OrderId,
                ShipmentId = request.ShipmentId,
            };

            await _publishEndPoint.Publish(orderFulfillmentCompletedEvent);

            return Success(HttpStatusCode.Accepted);
        }
    }
}
