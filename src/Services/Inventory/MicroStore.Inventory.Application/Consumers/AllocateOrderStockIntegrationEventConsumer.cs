using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.Domain.Exceptions;
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

            var result = await _orderCommandService.AllocateOrderStockAsync(new OrderStockModel
            {
                Items = MapeOrderItems(context.Message.Items),
            });


            if (result.IsFailure)
            {
                var details = ((OrderStockException)result.Exception).Details;

                await context.Publish(new StockRejectedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNumber = context.Message.OrderNumber,
                    UserId= context.Message.UserId, 
                    Details = details
                });

            }
            else
            {
                await context.Publish(new StockConfirmedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNumber = context.Message.OrderNumber,
                    UserId = context.Message.UserId
                });
            }

        }

        private List<OrderItemModel> MapeOrderItems(List<MicroStore.Inventory.IntegrationEvents.Models.OrderItemModel> products)
        {
            return products.Select(x => new OrderItemModel
            {
               ProductId= x.ProductId, 
               Quantity = x.Quantity
            }).ToList();
        }
    }
}
