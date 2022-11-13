using FluentAssertions;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Inventory.IntegrationEvents.Models;

namespace MicroStore.Inventory.Application.Tests.Consumers
{
    public class When_release_order_stock_integration_Event_consumed : BaseTestFixture
    {


        [Test]
        public async Task Should_release_allocated_order_stock()
        {

            List<Product> fakeProducts = await GenerateFakeProducts();


            ReleaseOrderStockIntegrationEvent integrationEvent = new ReleaseOrderStockIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                Products = fakeProducts.Select(x => new ProductModel
                {
                    ProductId = x.Id,
                    Quantity = 5
                }).ToList()
            };


            await TestHarness.Bus.Publish(integrationEvent);

            Assert.That(await TestHarness.Consumed.Any<ReleaseOrderStockIntegrationEvent>());


            await fakeProducts.ForEachAsync(async pr =>
            {
                var product = await Find<Product>(x => x.Id == pr.Id);

                product.AllocatedStock.Should().Be(0);

            });

        }


        private async Task<List<Product>> GenerateFakeProducts()
        {
            List<Product> products = new List<Product>();

            for(int i = 0; i< 5; i++)
            {
                Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 10);

                product.AllocateStock(5);

                await Insert(product);

                products.Add(product);
            }

            return products;
        }
    }
}
