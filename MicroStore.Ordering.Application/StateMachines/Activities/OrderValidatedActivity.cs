using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderValidatedActivity : IStateMachineActivity<OrderStateEntity, OrderValidatedEvent>, ITransientDependency
    {
        private readonly ILogger<OrderValidatedActivity> _logger;

        public OrderValidatedActivity(ILogger<OrderValidatedActivity> logger)
        {
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderValidatedEvent> context, IBehavior<OrderStateEntity, OrderValidatedEvent> next)
        {
            _logger.LogDebug("Executing Order Validated Activity");

            AcceptPaymentIntegationEvent integrationEvent = new AcceptPaymentIntegationEvent
            {
                TransactionId = context.Saga.TransactionId
            };

            return context.Publish(integrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderValidatedEvent, TException> context, IBehavior<OrderStateEntity, OrderValidatedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-validated");
        }
    }
}
