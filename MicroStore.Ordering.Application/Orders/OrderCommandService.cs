using MassTransit;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
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

        public async Task<Result<OrderSubmitedDto>> CreateOrderAsync(CreateOrderModel model, CancellationToken cancellationToken = default)
        {
            var orderSubmitedEvent = new OrderSubmitedEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = DateTime.UtcNow.Ticks.ToString(),
                ShippingAddress = model.ShippingAddress,
                BillingAddress = model.BillingAddress,
                UserName = model.UserId,
                ShippingCost = model.ShippingCost,
                TaxCost = model.TaxCost,
                SubTotal = model.SubTotal,
                TotalPrice = model.TotalPrice,
                SubmissionDate =DateTime.UtcNow,
                OrderItems = model.Items,
            };


            await _publishEndPoint.Publish(orderSubmitedEvent, cancellationToken);

            return PrepareSubmitOrderResponse(orderSubmitedEvent);
        }
        public async Task<Result<Unit>> FullfillOrderAsync(Guid orderId, FullfillOrderModel model, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(OrderStateEntity), orderId));
            }

            if (order.CurrentState != OrderStatusConst.Approved)
            {
                return new Result<Unit>(new BusinessException($"invalid order status. " +
                    $"please make sure that order is in {OrderStatusConst.Approved} status to be able to fullfill the order"));
            }

            var orderFulfillmentCompletedEvent = new OrderFulfillmentCompletedEvent
            {
                OrderId = orderId,
                ShipmentId = model.ShipmentId,
            };

            await _publishEndPoint.Publish(orderFulfillmentCompletedEvent);

            return Unit.Value;
        }
        public async Task<Result<Unit>> CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrder(orderId);


            if (order == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(OrderStateEntity), orderId));
            }
            if (order.CurrentState != OrderStatusConst.Fullfilled)
            {
                return new Result<Unit>(new BusinessException($"invalid order status. " + $"please make sure that order is in {OrderStatusConst.Fullfilled} status to be able to complete the order"));
            }

            var orderCompletedEvent = new OrderCompletedEvent
            {
                OrderId = orderId,
                ShippedDate = DateTime.UtcNow
            };

            await _publishEndPoint.Publish(orderCompletedEvent, cancellationToken);

            return Unit.Value;
        }

        public async Task<Result<Unit>> CancelOrderAsync(Guid orderId,CancelOrderModel model ,CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(OrderStateEntity), orderId));
            }

            if (order.CurrentState == OrderStatusConst.Cancelled)
            {
                return new Result<Unit>(new BusinessException("order state is already canceled"));
                 }

            var orderCancelledEvent = new OrderCancelledEvent
            {
                OrderId = orderId,
                Reason = model.Reason,
                CancellationDate = DateTime.UtcNow,
            };

            await _publishEndPoint.Publish(orderCancelledEvent, cancellationToken);

            return Unit.Value;
        }


        private OrderSubmitedDto PrepareSubmitOrderResponse(OrderSubmitedEvent orderSubmitedEvent)
        {
            return new OrderSubmitedDto
            {

                Id = orderSubmitedEvent.OrderId,
                OrderNumber = orderSubmitedEvent.OrderNumber,
                ShippingAddress = orderSubmitedEvent.ShippingAddress,
                BillingAddress = orderSubmitedEvent.BillingAddress,
                UserId = orderSubmitedEvent.UserName,
                ShippingCost = orderSubmitedEvent.ShippingCost,
                TaxCost = orderSubmitedEvent.TaxCost,
                SubTotal = orderSubmitedEvent.SubTotal,
                TotalPrice = orderSubmitedEvent.TotalPrice,
                SubmissionDate = orderSubmitedEvent.SubmissionDate,
                OrderItems = orderSubmitedEvent.OrderItems
            };
        }
    }
}
