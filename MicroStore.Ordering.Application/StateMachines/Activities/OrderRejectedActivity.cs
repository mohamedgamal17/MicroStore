using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderRejectedActivity : IStateMachineActivity<OrderStateEntity, OrderRejectedEvent>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderRejectedEvent> context, IBehavior<OrderStateEntity, OrderRejectedEvent> next)
        {
            return context.Publish(new RefundUserIntegrationEvent
            {
                TransactionId = context.Saga.TransactionId
            });
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderRejectedEvent, TException> context, IBehavior<OrderStateEntity, OrderRejectedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-rejected");
        }
    }
}
