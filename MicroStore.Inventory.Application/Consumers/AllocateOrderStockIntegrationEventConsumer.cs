using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.IntegrationEvents;

namespace MicroStore.Inventory.Application.Consumers
{
    public class AllocateOrderStockIntegrationEventConsumer : IConsumer<AllocateOrderStockIntegrationEvent>
    {


        private readonly ILogger<AllocateOrderStockIntegrationEventConsumer> _logger;


        private readonly IOrderCommandService _orderCommandService;


        public AllocateOrderStockIntegrationEventConsumer(ILogger<AllocateOrderStockIntegrationEventConsumer> logger, IOrderCommandService orderCommandService)
        {
            _logger = logger;
            _orderCommandService = orderCommandService;
        }

        public async Task Consume(ConsumeContext<AllocateOrderStockIntegrationEvent> context)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Consuming {EventName} : For Order : {OrderId}", nameof(AllocateOrderStockIntegrationEvent), context.Message.OrderId);
            }

            var result = await _orderCommandService.AllocateOrderStockAsync(new AllocateOrderStockModel
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                PaymentId = context.Message.PaymentId,
                UserId = context.Message.UserId,
                ShippingAddress = MapAddressModel(context.Message.ShippingAddress),
                BillingAddres = MapAddressModel(context.Message.BillingAddres),
                ShippingCost = context.Message.ShippingCost,
                TaxCost = context.Message.TaxCost,
                SubTotal = context.Message.SubTotal,
                TotalPrice = context.Message.TotalPrice,
                Items = MapeOrderItems(context.Message.Items),
            });


            if (result.IsFailure)
            {

                await context.Publish(new StockRejectedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNumber = context.Message.OrderNumber,
                    PaymentId = context.Message.PaymentId,
                    UserId= context.Message.UserId, 
                    Details = result.Error.Message
                });

            }
            else
            {
                await context.Publish(new StockConfirmedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNumber = context.Message.OrderNumber,
                    PaymentId= context.Message.PaymentId,
                    UserId = context.Message.UserId
                });
            }

        }

        private List<OrderItemModel> MapeOrderItems(List<MicroStore.Inventory.IntegrationEvents.Models.OrderItemModel> products)
        {
            return products.Select(x => new OrderItemModel
            {
               ItemId  = x.ItemId,
               ProductId= x.ProductId,
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
