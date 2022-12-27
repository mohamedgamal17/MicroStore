using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;
using MicroStore.Ordering.IntegrationEvents.Models;

namespace MicroStore.Ordering.Application.Tests.Consumers
{
    [NonParallelizable]
    public class When_submited_order_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_submited_event()
        {
            await TestHarness.Bus.Publish(new SubmitOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ShippingAddress = new AddressModel(),
                BillingAddress = new AddressModel(),
                SubmissionDate = DateTime.UtcNow,
                UserId = Guid.NewGuid().ToString(),
                OrderItems = new List<OrderItemModel>
                {
                    new OrderItemModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        Sku = Guid.NewGuid().ToString(),
                        ExternalProductId =Guid.NewGuid().ToString(),
                        Quantity = 50,
                        UnitPrice = 50
                    }
                }
            });


            Assert.That(await TestHarness.Published.Any<OrderSubmitedEvent>());
        }
    }
}
