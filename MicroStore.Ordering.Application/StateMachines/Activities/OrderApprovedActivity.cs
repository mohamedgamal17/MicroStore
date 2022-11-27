using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;
using MicroStore.Payment.IntegrationEvents.Models;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderApprovedActivity : IStateMachineActivity<OrderStateEntity, OrderApprovedEvent>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public  Task Execute(BehaviorContext<OrderStateEntity, OrderApprovedEvent> context, IBehavior<OrderStateEntity, OrderApprovedEvent> next)
        {
            var createPaymentIntegrationEvent = new CreatePaymentRequestIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId.ToString(),
                OrderNumber = context.Saga.OrderNumber,
                UserId = context.Saga.UserId,
                ShippingCost = context.Saga.ShippingCost,
                TaxCost = context.Saga.TaxCost,
                SubTotal = context.Saga.SubTotal,
                TotalCost = context.Saga.Total,
                Items = MapOrderItems(context.Saga.OrderItems)
            };

            return context.Publish(createPaymentIntegrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderApprovedEvent, TException> context, IBehavior<OrderStateEntity, OrderApprovedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-approved");
        }


        private List<PaymentRequestProductModel> MapOrderItems(List<OrderItemEntity> items) 
        {
            return items.Select(x => new PaymentRequestProductModel
            {
                ProductId = x.ProductId.ToString(),
                Name = x.ItemName,
                Sku = x.ProductId.ToString(),
                Image = x.ItemName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity

            }).ToList();

        }

    }
}
