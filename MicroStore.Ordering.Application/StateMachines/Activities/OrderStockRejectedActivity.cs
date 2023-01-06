using MassTransit;
using MicroStore.Ordering.Application.Abstractions.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderStockRejectedActivity : IStateMachineActivity<OrderStateEntity, OrderStockRejectedEvent>, ITransientDependency
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderStockRejectedEvent> context, IBehavior<OrderStateEntity, OrderStockRejectedEvent> next)
        {
            return context.Publish(new RefundPaymentIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId.ToString(),
                CustomerId = context.Saga.UserId,
                PaymentId = context.Saga.PaymentId
            });
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderStockRejectedEvent, TException> context, IBehavior<OrderStateEntity, OrderStockRejectedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-stock-rejected");
        }
    }
}
