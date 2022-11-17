using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    [Obsolete]
    public class OrderStockRejectedActivity : IStateMachineActivity<OrderStateEntity, OrderStockRejectedEvent>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderStockRejectedEvent> context, IBehavior<OrderStateEntity, OrderStockRejectedEvent> next)
        {
            return context.Publish(new RefundUserIntegrationEvent
            {
                TransactionId = context.Saga.PaymentId
            });
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderStockRejectedEvent, TException> context, IBehavior<OrderStateEntity, OrderStockRejectedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-stock-rejectd");
        }
    }
}
