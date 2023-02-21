using MassTransit;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderCancelledActivity : IStateMachineActivity<OrderStateEntity, OrderCancelledEvent>, ITransientDependency
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateEntity, OrderCancelledEvent> context, IBehavior<OrderStateEntity, OrderCancelledEvent> next)
        {
            await context.Publish(new RefundPaymentIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId.ToString(),
                CustomerId  =context.Saga.UserId,
                PaymentId = context.Saga.PaymentId
            });

            await context.Publish(new ReleaseOrderStockIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId.ToString(),
                OrderNumber = context.Saga.OrderNumber,
                PaymentId = context.Saga.PaymentId,
                UserId = context.Saga.UserId

            });
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderCancelledEvent, TException> context, IBehavior<OrderStateEntity, OrderCancelledEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-cancelled");
        }
    }
}
