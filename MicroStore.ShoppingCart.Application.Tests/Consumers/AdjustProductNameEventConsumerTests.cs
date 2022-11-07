using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.ShoppingCart.Application.Tests.Consumers
{
    public class AdjustProductNameEventConsumerTests : BaseTestBase
    {
        [Test]
        public async Task ShouldConsumeAdjustProductNameIntegrationEvent()
        {

            using var scope = ServiceProvider.CreateScope();

            ITestHarness harness = scope.ServiceProvider.GetRequiredService<ITestHarness>();

            await harness.Start();

            Product fakeProduct = await InsertFakeProduct();

            await harness.Bus.Publish(new AdjustProductNameIntegrationEvent(fakeProduct.Id, "NewAdjustedName"));

            Assert.IsTrue(await harness.Consumed.Any<AdjustProductNameIntegrationEvent>());

            Product product = await GetProductById(fakeProduct.Id);

            product.Name.Should().Be("NewAdjustedName");

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
