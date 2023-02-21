using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Tests.Consumers
{
    public class When_product_update_integration_event_consumer : BaseTestFixture
    {

        [Test]
        public async Task Should_update_product()
        {
            Product fakeProduct = await GenerateFakeProduct();

            var integrationEvent = new ProductUpdatedIntegerationEvent
            {
                ProductId = fakeProduct.Id,
                Name = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Thumbnail = Guid.NewGuid().ToString(),
                UnitPrice = 50
            };


            await TestHarness.Bus.Publish(integrationEvent);

            await TestHarness.Consumed.Any<ProductUpdatedIntegerationEvent>();

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.Name.Should().Be(integrationEvent.Name);
            product.Sku.Should().Be(integrationEvent.Sku);
            product.Thumbnail.Should().Be(integrationEvent.Thumbnail);
        }



        public Task<Product> GenerateFakeProduct()
            => WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.InsertAsync(new Product( Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0));
            });
    }
}
