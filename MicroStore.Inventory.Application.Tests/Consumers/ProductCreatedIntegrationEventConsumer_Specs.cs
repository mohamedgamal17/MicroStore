﻿using FluentAssertions;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Application.Tests.Consumers
{
    [NonParallelizable]
    public class When_product_create_intgeration_event_consumed : BaseTestFixture
    {
        [Test]
        public async Task Should_dispatch_product()
        {
            var integrationEvent = new ProductCreatedIntegrationEvent
            {
                ProductId = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                Thumbnail = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = 50
            };


            await TestHarness.Bus.Publish(integrationEvent);

            await TestHarness.Consumed.Any<ProductCreatedIntegrationEvent>();

            Product product = await Find<Product>(x => x.ExternalProductId == integrationEvent.ProductId);

            product.ExternalProductId.Should().Be(integrationEvent.ProductId);
            product.Sku.Should().Be(integrationEvent.Sku);
            product.Thumbnail.Should().Be(integrationEvent.Thumbnail);
            product.Name.Should().Be(integrationEvent.Name);
        }

    }
}