using MassTransit;
using MicroStore.Ordering.Application.StateMachines.Activities;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;

namespace MicroStore.Ordering.Application.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateEntity>
    {
        public State Submitted { get; private set; }
        public State Approved { get; private set; } 
        public State Pending { get; set; }
        public State Processing { get; private set; } 
        public State Completed { get; private set; }
        public State Cancelled { get; private set; }

        public Event<OrderSubmitedEvent> OrderSubmitted { get; private set; }
        public Event<OrderApprovedEvent> OrderApproved { get; private set; }
        public Event<OrderPaymentCreatedEvent> OrderPaymentCreated { get; set; }
        public Event<OrderPaymentAcceptedEvent> OrderPaymentAccepted { get; private set; }
        public Event<OrderCompletedEvent> OrderCompleted { get; private set; }
        public Event<OrderStockRejectedEvent> OrderStockRejected { get; private set; }
        public Event<OrderPaymentFaildEvent> OrderPaymentFaild { get; private set; }
        public Event<OrderShippmentFailedEvent> OrderShippmentFaild { get; private set; }
        public Event<OrderCancelledEvent> OrderCancelled { get; private set; }
      
        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x =>
            {
                x.CorrelateById(c => c.Message.OrderId);

                // x.InsertOnInitial = true;
            });

            Event(() => OrderApproved, x=> x.CorrelateById(c=> c.Message.OrderId));

            Event(() => OrderPaymentCreated, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderPaymentAccepted, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderCompleted, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderStockRejected, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderPaymentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderShippmentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderCancelled, x => x.CorrelateById(c => c.Message.OrderId));

           
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
                            context.Saga.CancellationDate = DateTime.UtcNow;
                            context.Saga.CancellationReason = context.Message.Details;
                        })
                        .TransitionTo(Cancelled)
                );


            During(Approved,
                    When(OrderPaymentCreated)
                        .Then((context) =>
                        {
                            context.Saga.PaymentId = context.Message.PaymentId;
                        })
                        .TransitionTo(Pending),

                    When(OrderPaymentFaild)
                        .Then((context) =>
                        {
                            context.Saga.CancellationDate = context.Message.FaultDate;
                        })
                        .TransitionTo(Cancelled)
                );

            During(Pending,
                    When(OrderPaymentAccepted)                       
                        .TransitionTo(Processing),

                    When(OrderPaymentFaild)
                        .Then((context) =>
                        {
                            context.Saga.CancellationDate = context.Message.FaultDate;
                            context.Saga.CancellationReason = context.Message.FaultReason;
                        })
                        .TransitionTo(Cancelled)
               );

            During(Processing,
                    When(OrderCompleted)
                        .Then((context) =>
                        {
                            context.Saga.ShippedDate = context.Message.ShippedDate;
                        })
                        .TransitionTo(Completed),


                   When(OrderShippmentFaild)
                        .Then((context) =>
                        {
                            context.Saga.CancellationReason = context.Message.Reason;
                            context.Saga.CancellationDate = context.Message.FaultDate;
                        })
                        .TransitionTo(Cancelled)
                        .Activity(x => x.OfType<OrderShippmentFaildActivity>())
                    );


            DuringAny(
                    When(OrderCancelled)
                        .Then((context) =>
                        {
                            context.Saga.CancellationDate = context.Message.CancellationDate;
                            context.Saga.CancellationReason = context.Message.Reason;
                        }).TransitionTo(Cancelled)
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
