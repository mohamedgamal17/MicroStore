using MassTransit;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.StateMachines.Activities;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.Events.Responses;

namespace MicroStore.Ordering.Application.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateEntity>
    {
        public State Submitted { get; private set; }     
        public State Accepted { get; set; }
        public State Approved { get; private set; }
        public State Fullfilled { get; private set; } 
        public State Completed { get; private set; }
        public State Cancelled { get; private set; }

        public Event<OrderSubmitedEvent> OrderSubmitted { get; private set; }
        public Event<OrderPaymentAcceptedEvent> OrderPaymentAccepted { get; private set; }
        public Event<OrderApprovedEvent> OrderApproved { get; private set; }     
        public Event<OrderFulfillmentCompletedEvent> OrderFullfilled { get; set; }
        public Event<OrderCompletedEvent> OrderCompleted { get; private set; }
        public Event<OrderStockRejectedEvent> OrderStockRejected { get; private set; }
        public Event<OrderPaymentFaildEvent> OrderPaymentFaild { get; private set; }
        public Event<OrderShippmentFailedEvent> OrderShippmentFaild { get; private set; }
        public Event<OrderCancelledEvent> OrderCancelled { get; private set; }

        public Event<CheckOrderStatusEvent> CheckOrderStatus { get; set; }

        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x =>
            {
                x.CorrelateById(c => c.Message.OrderId);

                // x.InsertOnInitial = true;
            });

            Event(() => OrderPaymentAccepted, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderApproved, x=> x.CorrelateById(c=> c.Message.OrderId));

            Event(() => OrderFullfilled, x => x.CorrelateById(c => c.Message.OrderId));
            
            Event(() => OrderCompleted, x => x.CorrelateById(c => c.Message.OrderId));
         
            Event(() => OrderStockRejected, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderPaymentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderShippmentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderCancelled, x => x.CorrelateById(c => c.Message.OrderId));


            Event(() => CheckOrderStatus , x =>
            {
                x.CorrelateById(c => c.Message.OrderId);

                x.OnMissingInstance((m) => m.Execute(async context =>
                {
                    await context.RespondAsync(StateMachineResult.Failure<OrderResponse>("order is not exist"));
                }));

            });

           
            InstanceState(x => x.CurrentState);

            Initially(
                    When(OrderSubmitted)
                        .CopyDataToInstance()
                        .TransitionTo(Submitted)
                        .Respond((context) => context.Saga.MapOrderSubmitedResponse())
                );



            During(Submitted,
                    When(OrderPaymentAccepted)
                        .Then((context) =>
                        {
                            context.Saga.PaymentId = context.Message.PaymentId;
                        })
                        .TransitionTo(Accepted)
                        .Activity(x => x.OfType<OrderAcceptedActivity>()),

                    When(OrderPaymentFaild)
                        .Then((context) =>
                        {
                            context.Saga.CancellationDate = context.Message.FaultDate;
                        })
                        .TransitionTo(Cancelled)

                );


            

            During(Accepted,
                    When(OrderApproved)
                        .TransitionTo(Approved),
                  
                    When(OrderStockRejected)
                        .Then((context) =>
                        {
                            context.Saga.CancellationDate = DateTime.UtcNow;
                            context.Saga.CancellationReason = context.Message.Details;
                        })
                        .TransitionTo(Cancelled)
                        .Activity(x => x.OfType<OrderStockRejectedActivity>())             
                );


      
            During(Approved,
                    When(OrderFullfilled)
                         .Then((context) =>
                         {
                             context.Saga.ShipmentId = context.Message.ShipmentId;
                             context.Saga.ShipmentSystem = context.Message.ShipmentSystem;
                         })                     
                        .TransitionTo(Fullfilled)
                        .RespondAsync((context)=> Task.FromResult(StateMachineResult.Success(context.Saga.MapOrderFullfilledResponse())))
                  );


            During(Fullfilled,
                    When(OrderCompleted)
                        .Then((context) =>
                        {
                            context.Saga.ShippedDate = context.Message.ShippedDate;
                        })
                        .TransitionTo(Completed)
                    );


            DuringAny(
                    When(OrderCancelled)
                        .Then((context) =>
                        {
                            context.Saga.CancellationDate = context.Message.CancellationDate;
                            context.Saga.CancellationReason = context.Message.Reason;

                        }).TransitionTo(Cancelled)
                        .Activity(x=>x.OfType<OrderCancelledActivity>()),

                     When(CheckOrderStatus)
                        .RespondAsync((context) => Task.FromResult(StateMachineResult.Success(context.Saga.MapOrderResponse())))
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
                x.Saga.ShippingCost = x.Message.ShippingCost;
                x.Saga.TaxCost = x.Message.TaxCost;
                x.Saga.SubTotal = x.Message.SubTotal;
                x.Saga.TotalPrice = x.Message.Total;
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
                ProductImage = item.ProductImage,
                Quantity = item.Quantity,
                ProductId = item.ProductId,
                UnitPrice = item.UnitPrice
            }).ToList();
        }

        public static OrderSubmitedResponse MapOrderSubmitedResponse(this OrderStateEntity orderStateEntity)
        {

            return new OrderSubmitedResponse
            {
                OrderId = orderStateEntity.CorrelationId,
                OrderNumber = orderStateEntity.OrderNumber,
                UserId = orderStateEntity.UserId,
                ShippingAddressId = orderStateEntity.ShippingAddressId,
                BillingAddressId = orderStateEntity.BillingAddressId,
                ShippingCost = orderStateEntity.ShippingCost,
                TaxCost = orderStateEntity.TaxCost,
                SubTotal = orderStateEntity.SubTotal,
                TotalPrice = orderStateEntity.TotalPrice,
                SubmissionDate = orderStateEntity.SubmissionDate,
                CurrentState = orderStateEntity.CurrentState,
                OrderItems = MapOrderItemResponse(orderStateEntity.OrderItems)
            };
        }


        public static OrderFullfilledResponse MapOrderFullfilledResponse(this OrderStateEntity orderStateEntity)
        {
            return new OrderFullfilledResponse
            {
                OrderId = orderStateEntity.CorrelationId,
                OrderNumber = orderStateEntity.OrderNumber,
                UserId = orderStateEntity.UserId,
                ShippingAddressId = orderStateEntity.ShippingAddressId,
                BillingAddressId = orderStateEntity.BillingAddressId,
                PaymentId = orderStateEntity.PaymentId,
                ShipmentId = orderStateEntity.ShipmentId,
                ShipmentSystem = orderStateEntity.ShipmentSystem,
                ShippingCost = orderStateEntity.ShippingCost,
                TaxCost = orderStateEntity.TaxCost,
                SubTotal = orderStateEntity.SubTotal,
                TotalPrice = orderStateEntity.TotalPrice,
                SubmissionDate = orderStateEntity.SubmissionDate,
                CurrentState = orderStateEntity.CurrentState,
                OrderItems = MapOrderItemResponse(orderStateEntity.OrderItems)
            };
        }


        public static OrderCompletedResponse MapOrderCompletedResponse(this OrderStateEntity orderStateEntity)
        {
            return new OrderCompletedResponse
            {
                OrderId = orderStateEntity.CorrelationId,
                OrderNumber = orderStateEntity.OrderNumber,
                UserId = orderStateEntity.UserId,
                ShippingAddressId = orderStateEntity.ShippingAddressId,
                BillingAddressId = orderStateEntity.BillingAddressId,
                PaymentId = orderStateEntity.PaymentId,
                ShipmentId = orderStateEntity.ShipmentId,
                ShipmentSystem = orderStateEntity.ShipmentSystem,
                ShippingCost = orderStateEntity.ShippingCost,
                TaxCost = orderStateEntity.TaxCost,
                SubTotal = orderStateEntity.SubTotal,
                TotalPrice = orderStateEntity.TotalPrice,
                SubmissionDate = orderStateEntity.SubmissionDate,
                ShippedDate = orderStateEntity.ShippedDate,
                CurrentState = orderStateEntity.CurrentState,
                OrderItems = MapOrderItemResponse(orderStateEntity.OrderItems)
            };
        }


        public static OrderResponse MapOrderResponse(this OrderStateEntity orderStateEntity)
        {
            return new OrderResponse
            {
                OrderId = orderStateEntity.CorrelationId,
                OrderNumber = orderStateEntity.OrderNumber,
                UserId = orderStateEntity.UserId,
                ShippingAddressId = orderStateEntity.ShippingAddressId,
                BillingAddressId = orderStateEntity.BillingAddressId,
                PaymentId = orderStateEntity.PaymentId,
                ShipmentId = orderStateEntity.ShipmentId,
                ShipmentSystem = orderStateEntity.ShipmentSystem,
                ShippingCost = orderStateEntity.ShippingCost,
                TaxCost = orderStateEntity.TaxCost,
                SubTotal = orderStateEntity.SubTotal,
                TotalPrice = orderStateEntity.TotalPrice,
                SubmissionDate = orderStateEntity.SubmissionDate,
                ShippedDate = orderStateEntity.ShippedDate,
                CurrentState = orderStateEntity.CurrentState,
                OrderItems = MapOrderItemResponse(orderStateEntity.OrderItems)
            };
        }

        private static List<OrderItemResponseModel> MapOrderItemResponse(List<OrderItemEntity> orderItems)
        {
            return orderItems.Select(item => new OrderItemResponseModel
            {

                 ItemName = item.ItemName,
                 ProductImage = item.ProductImage,
                 Quantity = item.Quantity,
                 ProductId = item.ProductId,
                 UnitPrice = item.UnitPrice,              
             }).ToList();
        }
    }
}
