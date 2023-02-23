using MassTransit;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines;

namespace MicroStore.Ordering.Application.Orders
{
    public class OrderCommandService : OrderApplicationService, IOrderCommandService
    {
        private readonly IPublishEndpoint _publishEndPoint;

        private readonly IOrderRepository _orderRepository;

        public OrderCommandService(IPublishEndpoint publishEndPoint, IOrderRepository orderRepository)
        {
            _publishEndPoint = publishEndPoint;
            _orderRepository = orderRepository;
        }

        public async Task<UnitResult<OrderSubmitedDto>> CreateOrderAsync(CreateOrderModel model, CancellationToken cancellationToken = default)
        {
            var orderSubmitedEvent = new OrderSubmitedEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ShippingAddress = model.ShippingAddress,
                BillingAddress = model.BillingAddress,
                UserId = model.UserId,
                ShippingCost = model.ShippingCost,
                TaxCost = model.TaxCost,
                SubTotal = model.SubTotal,
                TotalPrice = model.TotalPrice,
                SubmissionDate =DateTime.UtcNow,
                OrderItems = model.OrderItems,
            };


            await _publishEndPoint.Publish(orderSubmitedEvent, cancellationToken);

            return UnitResult.Success(PrepareSubmitOrderResponse(orderSubmitedEvent));
        }
        public async Task<UnitResult> FullfillOrderAsync(Guid orderId, FullfillOrderModel model, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
            {
                return UnitResult.Failure(ErrorInfo.NotFound($"Order entity with id {orderId} is not exist"));
            }

            if (order.CurrentState != OrderStatusConst.Approved)
            {
   
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"invalid order status. " +
                    $"please make sure that order is in {OrderStatusConst.Approved} status to be able to fullfill the order"));
            }

            var orderFulfillmentCompletedEvent = new OrderFulfillmentCompletedEvent
            {
                OrderId = orderId,
                ShipmentId = model.ShipmentId,
            };

            await _publishEndPoint.Publish(orderFulfillmentCompletedEvent);

            return UnitResult.Success();
        }
        public async Task<UnitResult> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
            {
                return UnitResult.Failure(ErrorInfo.NotFound($"Order entity with id {orderId} is not exist"));

            }
            if (order.CurrentState != OrderStatusConst.Fullfilled)
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"invalid order status. " + $"please make sure that order is in {OrderStatusConst.Fullfilled} status to be able to complete the order"));
            }

            var orderCompletedEvent = new OrderCompletedEvent
            {
                OrderId = orderId,
                ShippedDate = DateTime.UtcNow
            };

            await _publishEndPoint.Publish(orderCompletedEvent, cancellationToken);

            return UnitResult.Success();
        }

        public async Task<UnitResult> CancelOrderAsync(Guid orderId,CancelOrderModel model ,CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
            {
                return UnitResult.Failure(ErrorInfo.NotFound($"Order entity with id {orderId} is not exist"));
            }

            if (order.CurrentState == OrderStatusConst.Cancelled)
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic("order state is already canceled"));
            }

            var orderCancelledEvent = new OrderCancelledEvent
            {
                OrderId = orderId,
                Reason = model.Reason,
                CancellationDate = DateTime.UtcNow,
            };

            await _publishEndPoint.Publish(orderCancelledEvent, cancellationToken);

            return UnitResult.Success();
        }


        private OrderSubmitedDto PrepareSubmitOrderResponse(OrderSubmitedEvent orderSubmitedEvent)
        {
            return new OrderSubmitedDto
            {

                Id = orderSubmitedEvent.OrderId,
                OrderNumber = orderSubmitedEvent.OrderNumber,
                ShippingAddress = orderSubmitedEvent.ShippingAddress,
                BillingAddress = orderSubmitedEvent.BillingAddress,
                UserId = orderSubmitedEvent.UserId,
                ShippingCost = orderSubmitedEvent.ShippingCost,
                TaxCost = orderSubmitedEvent.TaxCost,
                SubTotal = orderSubmitedEvent.SubTotal,
                Total = orderSubmitedEvent.TotalPrice,
                SubmissionDate = orderSubmitedEvent.SubmissionDate,
                OrderItems = orderSubmitedEvent.OrderItems
            };
        }
    }
}
