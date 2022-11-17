using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Shipping.IntegrationEvent;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderPaymentAcceptedActivity : IStateMachineActivity<OrderStateEntity, OrderPaymentCompletedEvent>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateEntity, OrderPaymentCompletedEvent> context, IBehavior<OrderStateEntity, OrderPaymentCompletedEvent> next)
        {
            var createShippmentIntegrationEvent = new CreateShippmentIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId,
                OrderNumber = context.Saga.OrderNumber
            };

            await context.Publish(createShippmentIntegrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderPaymentCompletedEvent, TException> context, IBehavior<OrderStateEntity, OrderPaymentCompletedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-paid");
        }
    }
}
