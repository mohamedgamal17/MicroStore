using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.Events.Responses;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Commands
{
    public class SubmitOrderCommandHandler : CommandHandler<SubmitOrderCommand, OrderSubmitedDto>
    {
        private readonly IRequestClient<CheckOrderStatusEvent> _checkOrderRequestClinet;

        private readonly IPublishEndpoint _publishEndPoint;

        public SubmitOrderCommandHandler(IRequestClient<CheckOrderStatusEvent> checkOrderRequestClinet, IPublishEndpoint publishEndPoint)
        {
            _checkOrderRequestClinet = checkOrderRequestClinet;
            _publishEndPoint = publishEndPoint;
        }

        public override async Task<OrderSubmitedDto> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
        {
            SubmitOrderIntegrationEvent orderSubmitedEvent = new SubmitOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ShippingAddressId = request.ShippingAddressId,
                BillingAddressId = request.BillingAddressId,
                UserId = request.UserId,
                ShippingCost = request.ShippingCost,
                TaxCost = request.TaxCost,
                SubTotal = request.SubTotal,
                TotalPrice = request.TotalPrice,
                SubmissionDate = request.SubmissionDate,
                OrderItems = request.OrderItems.Select(x=> new IntegrationEvents.Models.OrderItemModel
                {
                    ItemName =x.ItemName,
                    ProductId =x.ProductId,
                    ProductImage = x.ProductImage,
                    Quantity =  x.Quantity,
                    UnitPrice = x.UnitPrice,
                }).ToList(),
            };


            await _publishEndPoint.Publish(orderSubmitedEvent, cancellationToken);

            return PrepareSubmitOrderResponse(orderSubmitedEvent);
        }


        private OrderSubmitedDto PrepareSubmitOrderResponse(SubmitOrderIntegrationEvent orderSubmitedEvent)
        {
            return new OrderSubmitedDto
            {

                OrderId = orderSubmitedEvent.OrderId,
                OrderNumber = orderSubmitedEvent.OrderNumber,
                ShippingAddressId = orderSubmitedEvent.ShippingAddressId,
                BillingAddressId = orderSubmitedEvent.BillingAddressId,
                UserId = orderSubmitedEvent.UserId,
                ShippingCost = orderSubmitedEvent.ShippingCost,
                TaxCost = orderSubmitedEvent.TaxCost,
                SubTotal = orderSubmitedEvent.SubTotal,
                Total = orderSubmitedEvent.TotalPrice,
                SubmissionDate = orderSubmitedEvent.SubmissionDate,
                OrderItems = PreapreOrderItems(orderSubmitedEvent.OrderItems)
            };
        }


        private List<OrderItemDto> PreapreOrderItems(List<IntegrationEvents.Models.OrderItemModel> orderItems)
        {
            return orderItems.Select(x => new OrderItemDto
            {
                ProductId = x.ProductId,
                ItemName = x.ItemName,
                ProductImage = x.ProductImage,
                Quantity =  x.Quantity,
                UnitPrice = x.UnitPrice,

            }).ToList();
        }
    }
}
