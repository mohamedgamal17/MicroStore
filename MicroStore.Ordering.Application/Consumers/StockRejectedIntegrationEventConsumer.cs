using MassTransit;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroStore.Ordering.Application.Consumers
{
    public class StockRejectedIntegrationEventConsumer : IConsumer<StockRejectedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<StockRejectedIntegrationEvent> context)
        {
            return context.Publish(new OrderStockRejectedEvent
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNubmer,
                Details = context.Message.Details
            });
        }
    }
}
