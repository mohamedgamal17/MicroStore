using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Models;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class UpdateProductCommandHandlerTest : BaseTestFixture
    {

        [Test]
        public async Task ShouldUpdateProduct()
        {

            var fakeCategory = await CreateFakeCategory();
            var fakeProduct = await CreateFakeProduct();
            var command = new UpdateProductCommand
            {
                ProductId = fakeProduct.Id,
                Sku = "NewSku",
                Name = "NewName",
                ShortDescription = "NewShortDescription",
                LongDescription = "NewLongDescription",
                Price = 120,
                OldPrice = 150,

                ProductCategories = new List<ProductCategoryModel>
                {
                    new ProductCategoryModel{CategoryId = fakeCategory.Id, IsFeatured = false}
                }

            };


            await Send(command);

            var product = await GetProductById(fakeProduct.Id);


            product.Sku.Should().Be(command.Sku);
            product.Name.Should().Be(command.Name);
            product.ShortDescription.Should().Be(command.ShortDescription);
            product.LongDescription.Should().Be(command.LongDescription);
            product.Price.Should().Be(command.Price);
            product.OldPrice.Should().Be(command.OldPrice);
            product.ProductCategories.First().CategoryId.Should().Be(fakeCategory.Id);
            product.ProductCategories.First().IsFeaturedProduct.Should().BeFalse();


            //Assert.That(await TestHarness.Published.Any<AdjustProductSkuIntegrationEvent>());
            //Assert.That(await TestHarness.Published.Any<AdjustProductNameIntegrationEvent>());
            //Assert.That(await TestHarness.Published.Any<AdjustProductPriceIntegrationEvent>());

        }

        private Task<Category> CreateFakeCategory()
        {

            return WithUnitOfWork(async (sp) =>
            {
                var fakeCategory = new Category("FakeCategory");
                var repository = sp.GetRequiredService<IRepository<Category>>();
                await repository.InsertAsync(fakeCategory);

                return fakeCategory;
            });

        }

        private  Task<Product> CreateFakeProduct()
        {

            return WithUnitOfWork(async (sp) =>
            {
                var fakeProduct = new Product("FakeSku", "FakeName", 50);
                var repository = sp.GetRequiredService<IRepository<Product>>();
                await repository.InsertAsync(fakeProduct);
                return fakeProduct;
            });
        }

        private Task<Product> GetProductById(Guid id)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();
                return repository.SingleAsync(x => x.Id == id);
            });
        }
    }
}
