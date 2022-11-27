﻿using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.StateMachines.Activities
{
    public class OrderShippmentFaildActivity : IStateMachineActivity<OrderStateEntity, OrderShippmentFailedEvent>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Task Execute(BehaviorContext<OrderStateEntity, OrderShippmentFailedEvent> context, IBehavior<OrderStateEntity, OrderShippmentFailedEvent> next)
        {
            return context.Publish(new RefundPaymentIntegrationEvent
            {
                OrderId = context.Saga.CorrelationId.ToString(),
                OrderNumber = context.Saga.OrderNumber,
                CustomerId = context.Saga.UserId,
                PaymentId = context.Saga.PaymentId,
            });
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateEntity, OrderShippmentFailedEvent, TException> context, IBehavior<OrderStateEntity, OrderShippmentFailedEvent> next) where TException : Exception
        {
            return next.Execute(context);
        }

        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }
    }
}
