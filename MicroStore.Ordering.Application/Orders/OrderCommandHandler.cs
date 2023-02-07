using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines;
using System.Net;
namespace MicroStore.Ordering.Application.Orders
{
    public class OrderCommandHandler : RequestHandler,
        ICommandHandler<SubmitOrderCommand, OrderSubmitedDto>,
        ICommandHandler<FullfillOrderCommand>,
        ICommandHandler<CompleteOrderCommand>,
        ICommandHandler<CancelOrderCommand>
    {


        private readonly IPublishEndpoint _publishEndPoint;

        private readonly IOrderRepository _orderRepository;

        public OrderCommandHandler(IPublishEndpoint publishEndPoint, IOrderRepository orderRepository)
        {
            _publishEndPoint = publishEndPoint;
            _orderRepository = orderRepository;
        }

        public async Task<ResponseResult<OrderSubmitedDto>> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
        {
            var orderSubmitedEvent = new OrderSubmitedEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ShippingAddress = request.ShippingAddress,
                BillingAddress = request.BillingAddress,
                UserId = request.UserId,
                ShippingCost = request.ShippingCost,
                TaxCost = request.TaxCost,
                SubTotal = request.SubTotal,
                TotalPrice = request.TotalPrice,
                SubmissionDate = request.SubmissionDate,
                OrderItems = request.OrderItems,
            };


            await _publishEndPoint.Publish(orderSubmitedEvent, cancellationToken);

            return Success(HttpStatusCode.Accepted, PrepareSubmitOrderResponse(orderSubmitedEvent));
        }

        public async Task<ResponseResult<Unit>> Handle(FullfillOrderCommand request, CancellationToken cancellationToken)
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

        public async Task<ResponseResult<Unit>> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrder(request.OrderId);

            if (order == null)
            {
                return Failure(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Order entity with id {request.OrderId} is not exist"
                });
            }
            if (order.CurrentState != OrderStatusConst.Fullfilled)
            {
                var errorInfo = new ErrorInfo
                {
                    Message = $"invalid order status. " + $"please make sure that order is in {OrderStatusConst.Fullfilled} status to be able to complete the order"
                };

                return Failure(HttpStatusCode.BadRequest, errorInfo);
            }

            var orderCompletedEvent = new OrderCompletedEvent
            {
                OrderId = request.OrderId,
                ShippedDate = request.ShipedDate
            };

            await _publishEndPoint.Publish(orderCompletedEvent, cancellationToken);

            return Success((HttpStatusCode.Accepted));
        }

        public async Task<ResponseResult<Unit>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrder(request.OrderId);

            if (order == null)
            {
                return Failure(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Order entity with id {request.OrderId} is not exist"
                });
            }

            if (order.CurrentState == OrderStatusConst.Cancelled)
            {
                return Failure(HttpStatusCode.BadRequest, new ErrorInfo
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

            return Success((HttpStatusCode.Accepted));
        }


        private AddressModel PrepareAddressModel(MicroStore.Ordering.IntegrationEvents.Models.AddressModel address)
        {
            return new AddressModel
            {
                CountryCode = address.CountryCode,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                Zip = address.Zip,
                Name = address.Name,
                Phone = address.Phone

            };
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
