using MassTransit;
using MicroStore.Ordering.Application.StateMachines.Activities;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;

namespace MicroStore.Ordering.Application.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateEntity>
    {
        //will introduce new state pendingpayment
        public State Submitted { get; private set; }
        public State Approved { get; private set; } 
        public State PendingPayment { get; set; }
        public State Paid { get; private set; } 
        public State Shipping { get; private set; }
        public State Completed { get; private set; }
        public State Faulted { get; private set; }
        public State Cancelled { get; private set; }

        public Event<OrderSubmitedEvent> OrderSubmitted { get; private set; }
        public Event<OrderApprovedEvent> OrderApproved { get; private set; }
        public Event<OrderPaymentCreatedEvent> OrderPaymentCreated { get; set; }
        public Event<OrderPaymentCompletedEvent> OrderPaid { get; private set; }
        public Event<OrderShippmentCreatedEvent> OrderShippmentCreated { get; private set; }
        public Event<OrderShippedEvent> OrderShipped { get; private set; }
        public Event<OrderStockRejectedEvent> OrderStockRejected { get; private set; }
        public Event<OrderPaymentFaildEvent> OrderPaymentFaild { get; private set; }
        public Event<OrderShippmentFailedEvent> OrderShippmentFaild { get; private set; }
        public Event<OrderCancelledEvent> OrderCancelled { get; private set; }

        public Event<CheckOrderRequest> CheckOrder { get; set; }
      

        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x =>
            {
                x.CorrelateById(c => c.Message.OrderId);

                // x.InsertOnInitial = true;
            });

            Event(() => OrderApproved, x=> x.CorrelateById(c=> c.Message.OrderId));

            Event(() => OrderPaymentCreated, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderPaid, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderShippmentCreated, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderShipped, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderStockRejected, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderPaymentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderShippmentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderCancelled, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => CheckOrder, x =>
            {
                x.CorrelateById(c => c.Message.OrderId);
                x.ReadOnly = true;
                x.OnMissingInstance(behaviour =>
                {
                    return behaviour.ExecuteAsync((consumerContext) => consumerContext.RespondAsync(new OrderNotFound
                    {
                        OrderId = consumerContext.Message.OrderId
                    }));

                });
            });

            InstanceState(x => x.CurrentState);

            Initially(
                    When(OrderSubmitted)
                        .CopyDataToInstance()
                        .TransitionTo(Submitted)
                        .Activity(x => x.OfType<OrderSubmitedActivity>())
                        .Respond((context) => new OrderSubmitedResponse()
                        {
                            OrderId = context.Saga.CorrelationId,
                            OrderNumber = context.Saga.OrderNumber,
                            UserId = context.Saga.UserId,
                            ShippingAddressId = context.Saga.ShippingAddressId,
                            BillingAddressId = context.Saga.BillingAddressId,
                            Total = context.Saga.Total,
                            SubTotal = context.Saga.SubTotal,
                            SubmissionDate = context.Saga.SubmissionDate,
                            Items = context.Saga.OrderItems.MapOrderItemsToModel(),
                        })
                        
                );

         
            During(Submitted,
                    When(OrderApproved)
                        .TransitionTo(Approved)
                        .Activity(x=> x.OfType<OrderApprovedActivity>()),
                  
                    When(OrderStockRejected)
                        .Then((context) =>
                        {
                            context.Saga.FaultDate = DateTime.UtcNow;
                            context.Saga.FaultReason = context.Message.Details;
                        })
                        .TransitionTo(Faulted)
                );


            During(Approved,
                    When(OrderPaymentCreated)
                        .Then((context) =>
                        {
                            context.Saga.PaymentId = context.Message.PaymentId;
                        })
                        .TransitionTo(PendingPayment),

                    When(OrderPaymentFaild)
                        .Then((context) =>
                        {
                            context.Saga.FaultDate = context.Message.FaultDate;
                        })
                        .TransitionTo(Faulted)
                );

            During(PendingPayment,
                    When(OrderPaid)
                        .Then((context) =>
                        {
                            context.Saga.PaymentAcceptedDate = context.Message.PaymentAcceptedDate;
                        })
                        .TransitionTo(Paid)
                        .Activity(x => x.OfType<OrderPaymentAcceptedActivity>()),

                    When(OrderPaymentFaild)
                        .Then((context) =>
                        {
                            context.Saga.FaultDate = context.Message.FaultDate;
                            context.Saga.FaultReason = context.Message.FaultReason;
                        })
                        .TransitionTo(Faulted)
               );

            During(Paid,
                    When(OrderShippmentCreated)
                        .Then((context) =>
                        {
                            context.Saga.ShippmentId = context.Message.ShippmentId;
                        })
                        .TransitionTo(Shipping)
                       
                    );


            During(Shipping,
                   When(OrderShipped)
                        .Then((context) =>
                        {
                            context.Saga.ShippedDate = context.Message.ShippedDate;
                        })
                        .TransitionTo(Completed),

                   When(OrderShippmentFaild)
                        .Then((context) =>
                        {
                            context.Saga.FaultReason = context.Message.Reason;
                            context.Saga.FaultDate = context.Message.FaultDate;
                        })
                        .TransitionTo(Faulted)
                        .Activity(x => x.OfType<OrderShippmentFaildActivity>())
                    );


            DuringAny(
                    When(OrderCancelled)
                        .Then((context) =>
                        {
                            context.Saga.CancellationDate = context.Message.CancellationDate;
                            context.Saga.CancellationReason = context.Message.Reason;
                            context.Saga.CancelledBy = context.Message.CancelledBy;
                        }).TransitionTo(Cancelled)
                    );



            DuringAny(
                    When(CheckOrder)
                        .Respond((context) => new OrderResponse
                        {

                            OrderId = context.Saga.CorrelationId,
                            OrderNumber = context.Saga.OrderNumber,
                            BillingAddressId = context.Saga.BillingAddressId,
                            ShippingAddressId = context.Saga.ShippingAddressId,
                            PaymentId = context.Saga.PaymentId,
                            ShippmentId = context.Saga.ShippmentId,
                            UserId = context.Saga.UserId,
                            SubTotal = context.Saga.SubTotal,
                            Total = context.Saga.Total,
                            OrderItems = context.Saga.OrderItems.MapOrderItemsToModel(),
                            Status = context.Saga.CurrentState,
                            SubmissionDate = context.Saga.SubmissionDate,
                            PaymentAcceptedDate = context.Saga.PaymentAcceptedDate,
                            ConfirmationDate = context.Saga.ConfirmationDate,
                            ShippedDate = context.Saga.ShippedDate,
                            CancellationDate = context.Saga.CancellationDate,
                            CancelledBy = context.Saga.CancelledBy,
                            CancellationReason = context.Saga.CancellationReason,
                            FaultDate = context.Saga.FaultDate,
                            FaultReason = context.Saga.FaultReason,

                        })
                    );
        }

    }

    internal static class OrderStateMachineExtensions
    {
        public static EventActivityBinder<OrderStateEntity, OrderSubmitedEvent> CopyDataToInstance(this EventActivityBinder<OrderStateEntity, OrderSubmitedEvent> binder)
        {
            return binder.Then(x =>
            {
                x.Saga.CorrelationId = x.Message.OrderId;
                x.Saga.OrderNumber = x.Message.OrderNumber;
                x.Saga.UserId = x.Message.UserId;
                x.Saga.BillingAddressId = x.Message.BillingAddressId;
                x.Saga.ShippingAddressId = x.Message.ShippingAddressId;
                x.Saga.SubTotal = x.Message.SubTotal;
                x.Saga.Total = x.Message.Total;
                x.Saga.SubmissionDate = x.Message.SubmissionDate;
                x.Saga.OrderItems = x.Message.OrderItems.Select(item => new OrderItemEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList();

            });
        }

        public static List<OrderItemModel> MapOrderItemsToModel(this List<OrderItemEntity> orderItems)
        {
            return orderItems.Select(item =>
            new OrderItemModel
            {
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                ProductId = item.ProductId,
                UnitPrice = item.UnitPrice
            }).ToList();
        }



    }
}
