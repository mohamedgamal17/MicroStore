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
                _logger.LogDebug("Consuming {EventName} : For Order : {OrderId}", nameof(AllocateOrderStockIntegrationEvent), context.Message.OrderId);
            }

          
            var result = await _localMessageBus.Send(new AllocateOrderStockCommand
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                Products = MapeProducts(context.Message.Products)
            });

            if (result.IsFailure)
            {

                await context.Publish(new StockRejectedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNubmer = context.Message.OrderNumber,
                    Details = result.Error.ToString()
                });

            }
            else
            {
                await context.Publish(new StockConfirmedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNumber = context.Message.OrderNumber
                });
            }

        }

        private List<ProductModel> MapeProducts(List<MicroStore.Inventory.IntegrationEvents.Models.ProductModel> products)
        {
            return products.Select(x => new ProductModel
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();
        }
    }
}
