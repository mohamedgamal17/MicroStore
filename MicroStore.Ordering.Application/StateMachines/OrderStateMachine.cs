using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines.Activities;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

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

        public ICachedServiceProvider ServiceProvider { get; set; }

        public IObjectMapper ObjectMapper => ServiceProvider.GetRequiredService<IObjectMapper>();

        public Event<CheckOrderStatusEvent> CheckOrderStatus { get; set; }

        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x =>
            {
                x.CorrelateById(c => c.Message.OrderId);

                // x.InsertOnInitial = true;
            });

            Event(() => OrderPaymentAccepted, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderApproved, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderFullfilled, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderCompleted, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderStockRejected, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderPaymentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderShippmentFaild, x => x.CorrelateById(c => c.Message.OrderId));

            Event(() => OrderCancelled, x => x.CorrelateById(c => c.Message.OrderId));


            Event(() => CheckOrderStatus, x =>
            {
                x.CorrelateById(c => c.Message.OrderId);

                x.OnMissingInstance((m) => m.Execute(async context =>
                {
                    await context.RespondAsync(StateMachineResult.Failure<OrderDto>("order is not exist"));
                }));

            });


            InstanceState(x => x.CurrentState);

            Initially(
                    When(OrderSubmitted)
                        .CopyDataToInstance()
                        .TransitionTo(Submitted)
                        .Respond((context) => ObjectMapper.Map<OrderStateEntity, OrderDto>(context.Saga))
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

                            List<OrderItemEntity> items = context.Saga.OrderItems
                            .Where(x => context.Message.Details.ContainsKey(x.ExternalProductId))
                            .ToList();

                            StringBuilder stringBuilder = new StringBuilder();

                            items.ForEach(item => stringBuilder.AppendLine($"Product : {item.Name} has not required stock"));

                            context.Saga.CancellationReason = stringBuilder.ToString();
                        })
                        .TransitionTo(Cancelled)
                        .Activity(x => x.OfType<OrderStockRejectedActivity>())
                );



            During(Approved,
                    When(OrderFullfilled)
                         .Then((context) =>
                         {
                             context.Saga.ShipmentId = context.Message.ShipmentId;
                         })
                        .TransitionTo(Fullfilled)
                        .RespondAsync((context) => Task.FromResult(StateMachineResult.Success(ObjectMapper.Map<OrderStateEntity, OrderDto>(context.Saga))))
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
                        .Activity(x => x.OfType<OrderCancelledActivity>()),

                     When(CheckOrderStatus)
                        .RespondAsync((context) => Task.FromResult(StateMachineResult.Success(ObjectMapper.Map<OrderStateEntity, OrderDto>(context.Saga))))
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
                x.Saga.UserId = x.Message.UserName;
                x.Saga.BillingAddress = MapAddress(x.Message.BillingAddress);
                x.Saga.ShippingAddress = MapAddress(x.Message.ShippingAddress);
                x.Saga.ShippingCost = x.Message.ShippingCost;
                x.Saga.TaxCost = x.Message.TaxCost;
                x.Saga.SubTotal = x.Message.SubTotal;
                x.Saga.TotalPrice = x.Message.TotalPrice;
                x.Saga.SubmissionDate = x.Message.SubmissionDate;
                x.Saga.OrderItems = x.Message.OrderItems.Select(item => new OrderItemEntity
                {
                    Id = Guid.NewGuid(),
                    ExternalProductId = item.ExternalProductId,
                    Sku = item.Sku,
                    Name = item.Name,
                    Thumbnail = item.Thumbnail,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                }).ToList();

            });
        }

        public static List<OrderItemModel> MapOrderItemsToModel(this List<OrderItemEntity> orderItems)
        {
            return orderItems.Select(item =>
            new OrderItemModel
            {
                ExternalProductId = item.ExternalProductId,
                Sku = item.Sku,
                Name = item.Name,
                Thumbnail = item.Thumbnail,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
            }).ToList();
        }

        public static Address MapAddress(AddressModel addressModel)
        {
            return new AddressBuilder()
                      .WithCountryCode(addressModel.CountryCode)
                      .WithCity(addressModel.City)
                      .WithState(addressModel.State)
                      .WithPostalCode(addressModel.PostalCode)
                      .WithAddressLine(addressModel.AddressLine1, addressModel.AddressLine2)
                      .WithName(addressModel.Name)
                      .WithPhone(addressModel.Phone)
                      .WithZip(addressModel.Zip)
                      .Build();
        }

    }
}
