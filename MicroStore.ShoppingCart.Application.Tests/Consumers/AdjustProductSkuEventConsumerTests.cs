using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.ShoppingCart.Application.Tests.Consumers
{
    public class AdjustProductSkuEventConsumerTests : BaseTestBase
    {


        [Test]
        public async Task ShouldConsumeAdjustProductSkuIntegrationEvent()
        {
            using var scope = ServiceProvider.CreateScope();

            ITestHarness harness = scope.ServiceProvider.GetRequiredService<ITestHarness>();

            await harness.Start();

            Product fakeProduct = await InsertFakeProduct();

            await harness.Bus.Publish(new AdjustProductSkuIntegrationEvent(fakeProduct.Id, "NewAdjustedSku"));

            Assert.IsTrue(await harness.Consumed.Any<AdjustProductSkuIntegrationEvent>());

            await Task.Delay(1000);
 
            Product product = await GetProductById(fakeProduct.Id);

            product.Sku.Should().Be("NewAdjustedSku");
        }
        private Task<Product> InsertFakeProduct() => WithUnitOfWork(async (sp) =>
        {
            var repository = sp.GetRequiredService<IRepository<Product>>();
            var product = new Product(Guid.NewGuid(), "FakeName", "FakeSku", 50);
            await repository.InsertAsync(product);
            return product;
        });


        private Task<Product> GetProductById(Guid productId)
            => WithUnitOfWork(sp =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.SingleAsync(x => x.Id == productId);
            });
    }

}
