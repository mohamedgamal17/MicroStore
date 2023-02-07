using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Inventory.IntegrationEvents.Models;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderAcceptedActivity : IStateMachineActivity<OrderStateEntity, OrderPaymentAcceptedEvent>, ITransientDependency
    {
        private readonly ILogger<OrderAcceptedActivity> _logger;

        public OrderAcceptedActivity(ILogger<OrderAcceptedActivity> logger)
        {
            _logger = logger;
        }


        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderPaymentAcceptedEvent> context, IBehavior<OrderStateEntity, OrderPaymentAcceptedEvent> next)
        {
            _logger.LogDebug("Executing Order Confirmed Activity");

            AllocateOrderStockIntegrationEvent integrationEvent = new AllocateOrderStockIntegrationEvent
            {
                ExternalOrderId = context.Saga.CorrelationId.ToString(),
                OrderNumber = context.Saga.OrderNumber,
                ExternalPaymentId  = context.Saga.PaymentId,
                UserId = context.Saga.UserId,
                ShippingAddress = MapAddressModel(context.Saga.ShippingAddress),
                BillingAddres = MapAddressModel(context.Saga.BillingAddress),
                ShippingCost = context.Saga.ShippingCost,
                TaxCost = context.Saga.TaxCost,
                SubTotal = context.Saga.SubTotal,
                TotalPrice = context.Saga.TotalPrice,               
                Items = MapStockItems(context.Saga.OrderItems)
            };

            return context.Publish(integrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderPaymentAcceptedEvent, TException> context, IBehavior<OrderStateEntity, OrderPaymentAcceptedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("submited-order");
        }


        private List<OrderItemModel> MapStockItems(List<OrderItemEntity> stockItems)
        {
            return stockItems.Select(x => new OrderItemModel
            {
                ExternalItemId = x.Id.ToString(),
                ExternalProductId = x.ExternalProductId,
                Sku = x.Sku,
                Name = x.Name,
                Thumbnail = x.Thumbnail,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity
            }).ToList();
        }


        private AddressModel MapAddressModel(Address address)
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
    }
}
