using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_submited_order_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_submited_event()
        {
            await TestHarness.Bus.Publish(new SubmitOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ShippingAddressId = Guid.NewGuid(),
                BillingAddressId = Guid.NewGuid(),
                SubmissionDate = DateTime.UtcNow,
                UserId = Guid.NewGuid().ToString(),
                OrderItems = new List<IntegrationEvents.Models.OrderItemModel>
                {
                    new IntegrationEvents.Models.OrderItemModel
                    {
                        ItemName = "fakeitem",
                        ProductId = Guid.NewGuid(),
                        Quantity = 50,
                        UnitPrice = 50
                    }
                }
            });


            Assert.That(await TestHarness.Published.Any<OrderSubmitedEvent>());
        }
    }
}
