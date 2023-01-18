using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using System.Net;

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

        public override async Task<ResponseResult<OrderSubmitedDto>> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
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
                Total = request.TotalPrice,
                SubmissionDate = request.SubmissionDate,
                OrderItems = request.OrderItems,
            };


            await _publishEndPoint.Publish(orderSubmitedEvent, cancellationToken);

            return Success(HttpStatusCode.Accepted, PrepareSubmitOrderResponse(orderSubmitedEvent));
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
                Total = orderSubmitedEvent.Total,
                SubmissionDate = orderSubmitedEvent.SubmissionDate,
                OrderItems = orderSubmitedEvent.OrderItems
            };
        }
     
    }
}
