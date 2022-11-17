using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

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
            var createPaymentIntegrationEvent = new CreatePaymentIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId,
                OrderNumber = context.Saga.OrderNumber,
                CustomerId = context.Saga.UserId,
                TotalPrice = context.Saga.Total
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
    }
}
