using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.IntegrationEvents;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Consumers
{
    public class AllocateOrderStockIntegrationEventConsumer : IConsumer<AllocateOrderStockIntegrationEvent>
    {

        private readonly ILogger<AllocateOrderStockIntegrationEventConsumer> _logger;

        private readonly IRepository<Product> _productRepository;

        public AllocateOrderStockIntegrationEventConsumer(IRepository<Product> productRepository, ILogger<AllocateOrderStockIntegrationEventConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AllocateOrderStockIntegrationEvent> context)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Consuming {EventName} : For Order : {OrderId}", nameof(AllocateOrderStockIntegrationEvent), context.Message.OrderId);
            }

            List<Result> failureResults = new List<Result>();

            foreach (var orderItem in context.Message.Products)
            {
                Product product = await _productRepository.SingleAsync(x => x.Id == orderItem.ProductId);

                var result = product.CanAllocateQuantity(orderItem.Quantity);

                if (result.IsFailure)
                {
                    failureResults.Add(result);
                }

            }

            if (failureResults.Any())
            {

                await context.Publish(new StockRejectedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNubmer =context.Message.OrderNumber,
                    Details = failureResults.Select(x=> x.Error)
                        .Aggregate((t1,t2)=> t1 + "\n" + t2)
                });


                return;
            }


            foreach (var orderItem in context.Message.Products)
            {
                Product product = await _productRepository.SingleOrDefaultAsync(x => x.Id == orderItem.ProductId);

                product.AllocateStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product);
            }


            await context.Publish(new StockConfirmedIntegrationEvent
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber
            });
           
        }
    }
}
