using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.ShoppingCart.Application.Tests.Consumers
{
    public class CreateProductEventConsumerTests : BaseTestBase
    {


        [Test]
        public async Task ShouldConsumeCreateProductIntegrationEvent()
        {
            using var scope = ServiceProvider.CreateScope();

            ITestHarness harness = scope.ServiceProvider.GetRequiredService<ITestHarness>();

            await harness.Start();

            var integrationEvent = new CreateProductIntegrationEvent(Guid.NewGuid(), "FakeName", "FakeSku", 65);

            await harness.Bus.Publish(integrationEvent);

            Assert.IsTrue(await harness.Consumed.Any<CreateProductIntegrationEvent>());

            var productCount = await CountAsync();

            productCount.Should().Be(1);

        }

        private Task<int> CountAsync()
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();
                return repository.CountAsync();
            });
        }
    }
}
