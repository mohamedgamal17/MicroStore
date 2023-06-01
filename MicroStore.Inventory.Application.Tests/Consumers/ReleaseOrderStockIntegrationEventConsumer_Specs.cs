﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.Domain.ValueObjects;
using MicroStore.Inventory.IntegrationEvents;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Tests.Consumers
{
   
    public class When_release_order_stock_integration_Event_consumed : BaseTestFixture
    {


        [Test]
        public async Task Should_release_allocated_order_stock()
        {


            Order fakeOrder = await CreateFakeOrder();


            ReleaseOrderStockIntegrationEvent integrationEvent = new ReleaseOrderStockIntegrationEvent
            {
                OrderId = fakeOrder.Id,
                OrderNumber = fakeOrder.OrderNumber,

                PaymentId = fakeOrder.PaymentId,

                UserId = fakeOrder.UserId
 
            };


            await TestHarness.Bus.Publish(integrationEvent);

            Assert.That(await TestHarness.Consumed.Any<ReleaseOrderStockIntegrationEvent>());

            var product =  await SingleAsync<Product>(x => x.Id == fakeOrder.Items.First().ProductId);

            product.AllocatedStock.Should().Be(0);

        }

        private async Task<Order> CreateFakeOrder()
        {
            var fakeProduct = await GenerateFakeProduct();

            Order order = new Order
            {

                OrderNumber = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                ShippingCost = 0,
                TaxCost = 0,
                SubTotal = 0,
                TotalPrice = 0,
                ShippingAddress = GenerateFakeAddress(),
                BillingAddres = GenerateFakeAddress(),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Name = fakeProduct.Name,
                        Sku = fakeProduct.Sku,
                        ProductId = fakeProduct.Id,
                        Thumbnail = fakeProduct.Thumbnail,
                        Quantity = fakeProduct.AllocatedStock,
                        UnitPrice = 50
                    }
           
                }
            };

            return await WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Order>>();

                return repository.InsertAsync(order);
            });
        }

        private List<OrderItem> GenerateFakeOrderItem(List<Product> products, int quantity)
        {
            return products.Select(x => new OrderItem
            {
           
            }).ToList();
        }


        private Address GenerateFakeAddress()
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

            }.AsAddressValueObject();
        }

        private async Task<Product> GenerateFakeProduct()
        {

            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 20);

            product.AllocateStock(20);


            return await Insert(product);
            
        }
    }
}
