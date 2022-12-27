using FluentAssertions;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Inventory.IntegrationEvents.Models;
namespace MicroStore.Inventory.Application.Tests.Consumers
{
    [NonParallelizable]

    public class When_allocate_order_stock_integration_event_consumed : BaseTestFixture
    {

        [Test]
        public async Task Should_allocate_product_quantity_and_publish_order_stock_confirmed_integration_event()
        {
            int allocatedQuantity = 5;

            List<Product> fakeProducts = await GenerateFakeProducts();

            var integrationEvent = new AllocateOrderStockIntegrationEvent
            {
                ExternalOrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                ExternalPaymentId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                ShippingCost= 0,
                TaxCost = 0,
                SubTotal = 0,
                TotalPrice = 0,
                ShippingAddress = GenerateFakeAddress(),
                BillingAddres = GenerateFakeAddress(),
                Items = GenerateFakeOrderItem(fakeProducts, allocatedQuantity)

            };

            await TestHarness.Bus.Publish(integrationEvent);

            Assert.That(await TestHarness.Consumed.Any<AllocateOrderStockIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<StockConfirmedIntegrationEvent>());

            await fakeProducts.ForEachAsync(async pr =>
            {
                var product = await Find<Product>(x => x.Id == pr.Id);

                product.AllocatedStock.Should().Be(allocatedQuantity);

            });
        }


        private AddressModel GenerateFakeAddress()
        {
            return new AddressModel
            {
                CountryCode = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString(),
                Zip = Guid.NewGuid().ToString(),
                AddressLine1 = Guid.NewGuid().ToString(),
                AddressLine2 = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),
            };
        }

        private List<OrderItemModel> GenerateFakeOrderItem(List<Product> products , int quantity)
        {
            return products.Select(x => new OrderItemModel
            {
                ExternalItemId = Guid.NewGuid().ToString(),
                Name = x.Name,
                Sku = x.Sku,
                ExternalProductId = x.ExternalProductId,
                Thumbnail = x.Thumbnail,
                Quantity = quantity,
                UnitPrice = 50
            }).ToList();
        }

        [Test]
        public async Task Should_publish_order_stock_rejected_when_avilable_stock_is_less_than_requested_stock()
        {
            int allocatedQuantity = 15;

            List<Product> fakeProducts = await GenerateFakeProducts();

            var integrationEvent = new AllocateOrderStockIntegrationEvent
            {
                ExternalOrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                ExternalPaymentId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                ShippingCost = 0,
                TaxCost = 0,
                SubTotal = 0,
                TotalPrice = 0,
                ShippingAddress = GenerateFakeAddress(),
                BillingAddres = GenerateFakeAddress(),
                Items = GenerateFakeOrderItem(fakeProducts, allocatedQuantity)

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

            fakeProducts.Add(await Insert(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString() ,Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5)));

            fakeProducts.Add(await Insert(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString() ,Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 10)));

            fakeProducts.Add(await Insert(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString() ,Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 15)));

            return fakeProducts;
        }



    }
}
