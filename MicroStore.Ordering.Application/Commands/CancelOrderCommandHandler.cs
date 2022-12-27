using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Consts;
using MicroStore.Ordering.Application.Abstractions.Interfaces;
using MicroStore.Ordering.Events;
using System.Net;
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

        public override async Task<ResponseResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {

            var order = await _orderRepository.GetOrder(request.OrderId);

            if(order == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Order entity with id {request.OrderId} is not exist"
                });
            }

            if(order.CurrentState == OrderStatusConst.Cancelled)
            {
                return ResponseResult.Failure((int)HttpStatusCode.BadRequest, new ErrorInfo
                {
                    Message = "order state is already canceled"
                });
            }

            var orderCancelledEvent = new OrderCancelledEvent
            {
                OrderId = request.OrderId,
                Reason = request.Reason,
                CancellationDate = request.CancellationDate,
            };

            await _publishEndPoint.Publish(orderCancelledEvent, cancellationToken);

            return ResponseResult.Success((int)(HttpStatusCode.Processing));
        }
    }
}
