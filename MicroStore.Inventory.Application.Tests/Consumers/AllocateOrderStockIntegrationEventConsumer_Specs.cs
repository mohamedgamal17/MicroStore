using FluentAssertions;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Inventory.IntegrationEvents.Models;
namespace MicroStore.Inventory.Application.Tests.Consumers
{

 
    public class When_allocate_order_stock_integration_event_consumed : BaseTestFixture
    {

        [Test]
        public async Task Should_allocate_product_quantity_and_publish_order_stock_confirmed_integration_event()
        {
            List<Product> fakeProducts = await GenerateFakeProducts();

            var integrationEvent = new AllocateOrderStockIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                Products = fakeProducts.Select(pr => new ProductModel
                {
                    ProductId = pr.Id,
                    Quantity = 5
                }).ToList()
            };

            await TestHarness.Bus.Publish(integrationEvent);

            Assert.That(await TestHarness.Consumed.Any<AllocateOrderStockIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<StockConfirmedIntegrationEvent>());

            await fakeProducts.ForEachAsync(async pr =>
            {
                var product = await Find<Product>(x => x.Id == pr.Id);

                product.AllocatedStock.Should().Be(5);

            });
        }

        [Test]
        public async Task Should_publish_order_stock_rejected_when_avilable_stock_is_less_than_requested_stock()
        {
            List<Product> fakeProducts = await GenerateFakeProducts();

            var integrationEvent = new AllocateOrderStockIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                Products = fakeProducts.Select(pr => new ProductModel
                {
                    ProductId = pr.Id,
                    Quantity = 12
                }).ToList()
            };


            await TestHarness.Bus.Publish(integrationEvent);


            Assert.That(await TestHarness.Consumed.Any<AllocateOrderStockIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<StockRejectedIntegrationEvent>());


            await fakeProducts.ForEachAsync(async pr =>
            {
                var product = await Find<Product>(x => x.Id == pr.Id);

                product.AllocatedStock.Should().Be(0);

            });
        }





        private async Task<List<Product>> GenerateFakeProducts()
        {

            List<Product> fakeProducts = new List<Product>();

            fakeProducts.Add(await Insert(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5)));

            fakeProducts.Add(await Insert(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 10)));

            fakeProducts.Add(await Insert(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 15)));

            return fakeProducts;
        }



    }
}
