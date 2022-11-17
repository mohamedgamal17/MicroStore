using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    [Obsolete]
    public class OrderValidatedActivity : IStateMachineActivity<OrderStateEntity, OrderApprovedEvent>, ITransientDependency
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

        public Task Execute(BehaviorContext<OrderStateEntity, OrderApprovedEvent> context, IBehavior<OrderStateEntity, OrderApprovedEvent> next)
        {
            _logger.LogDebug("Executing Order Validated Activity");

            AcceptPaymentIntegationEvent integrationEvent = new AcceptPaymentIntegationEvent
            {
                TransactionId = context.Saga.PaymentId
            };

            return context.Publish(integrationEvent);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderApprovedEvent, TException> context, IBehavior<OrderStateEntity, OrderApprovedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-validated");
        }
    }
}
