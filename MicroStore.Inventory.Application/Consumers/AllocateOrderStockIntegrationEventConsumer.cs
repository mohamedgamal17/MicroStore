using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Common;
using MicroStore.Inventory.IntegrationEvents;

namespace MicroStore.Inventory.Application.Consumers
{
    public class AllocateOrderStockIntegrationEventConsumer : IConsumer<AllocateOrderStockIntegrationEvent>
    {


        private readonly ILogger<AllocateOrderStockIntegrationEventConsumer> _logger;

        private readonly ILocalMessageBus _localMessageBus;

        public AllocateOrderStockIntegrationEventConsumer(ILocalMessageBus localMessageBus, ILogger<AllocateOrderStockIntegrationEventConsumer> logger)
        {
            _localMessageBus = localMessageBus;
            _logger = logger;
        }



        public async Task Consume(ConsumeContext<AllocateOrderStockIntegrationEvent> context)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Consuming {EventName} : For Order : {OrderId}", nameof(AllocateOrderStockIntegrationEvent), context.Message.ExternalOrderId);
            }

          
            var result = await _localMessageBus.Send(new AllocateOrderStockCommand
            {
                ExternalOrderId = context.Message.ExternalOrderId,
                OrderNumber = context.Message.OrderNumber,
                ExternalPaymentId = context.Message.ExternalPaymentId,
                UserId = context.Message.UserId,
                ShippingAddress = MapAddressModel(context.Message.ShippingAddress),
                BillingAddres = MapAddressModel(context.Message.BillingAddres),
                ShippingCost = context.Message.ShippingCost,
                TaxCost = context.Message.TaxCost,
                SubTotal= context.Message.SubTotal,
                TotalPrice = context.Message.TotalPrice,
                Items = MapeOrderItems(context.Message.Items),
            });

            if (result.IsFailure)
            {

                await context.Publish(new StockRejectedIntegrationEvent
                {
                    ExternalOrderId = context.Message.ExternalOrderId,
                    OrderNumber = context.Message.OrderNumber,
                    ExternalPaymentId = context.Message.ExternalPaymentId,
                    UserId= context.Message.UserId, 
                    Details = result.Envelope.Error.Message
                });

            }
            else
            {
                await context.Publish(new StockConfirmedIntegrationEvent
                {
                    ExternalOrderId = context.Message.ExternalOrderId,
                    OrderNumber = context.Message.OrderNumber,
                    ExternalPaymentId= context.Message.ExternalPaymentId,
                    UserId = context.Message.UserId
                });
            }

        }

        private List<OrderItemModel> MapeOrderItems(List<MicroStore.Inventory.IntegrationEvents.Models.OrderItemModel> products)
        {
            return products.Select(x => new OrderItemModel
            {
               ExternalItemId  = x.ExternalItemId,
               ExternalProductId= x.ExternalProductId,
               Sku = x.Sku,
               Name = x.Name,
               Thumbnail = x.Thumbnail,
               UnitPrice= x.UnitPrice,  
               Quantity = x.Quantity
            }).ToList();
        }

        private AddressModel MapAddressModel(MicroStore.Inventory.IntegrationEvents.Models.AddressModel addressModel)
        {
            return new AddressModel
            {
                Name = addressModel.Name,
                Phone = addressModel.Phone,
                CountryCode = addressModel.CountryCode,
                City = addressModel.City,
                State = addressModel.State,
                PostalCode = addressModel.PostalCode,
                Zip = addressModel.Zip,
                AddressLine1 = addressModel.AddressLine1,
                AddressLine2 = addressModel.AddressLine2,
            };
        }
    }
}
