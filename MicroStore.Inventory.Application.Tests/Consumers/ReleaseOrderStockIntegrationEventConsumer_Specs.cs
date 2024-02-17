using FluentAssertions;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.IntegrationEvents;
namespace MicroStore.Inventory.Application.Tests.Consumers
{
    [NonParallelizable]
    public class When_release_order_stock_integration_Event_consumed : BaseTestFixture
    {


        [Test]
        public async Task Should_release_allocated_order_stock()
        {


            var fakeProduct = await GenerateFakeProduct();


            ReleaseOrderStockIntegrationEvent integrationEvent = new ReleaseOrderStockIntegrationEvent
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),

                PaymentId = Guid.NewGuid().ToString(),

                UserId = Guid.NewGuid().ToString(),
                Items = new List<IntegrationEvents.Models.OrderItemModel>
                {
                    new IntegrationEvents.Models.OrderItemModel
                    {
                        ProductId = fakeProduct.Id,
                        Quantity = 20
                    }
                }

            };


            await TestHarness.Bus.Publish(integrationEvent);

            Assert.That(await TestHarness.Consumed.Any<ReleaseOrderStockIntegrationEvent>());

            var product =  await SingleAsync<Product>(x => x.Id == fakeProduct.Id);

            product.AllocatedStock.Should().Be(0);

        }

        private async Task<Product> GenerateFakeProduct()
        {

            Product product = new Product(100);

            product.AllocateStock(20);

            return await Insert(product);
            
        }
    }
}
