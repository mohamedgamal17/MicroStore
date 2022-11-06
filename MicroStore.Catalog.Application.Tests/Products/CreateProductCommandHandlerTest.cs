using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Models;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class CreateProductCommandHandlerTest : BaseTestFixture
    {

        [Test]
        public async Task ShouldCreateProduct()
        {
            var fakeCategory = await CreateFakeCategory();


            var request = new CreateProductCommand
            {
                Sku = "FakeSku",
                Name = "FakeName",
                ShortDescription = "FakeShortDescription",
                LongDescription = "FakeLongDesccription",
                Price = 50,
                OldPrice = 150,

            };

            request.ProductCategories
                .Add(new ProductCategoryModel { CategoryId = fakeCategory.Id, IsFeatured = false });

            var result = await Send(request);


            var productsConut = await GetProductsCount();

            var product = await GetProductById(result.ProductId);

            productsConut.Should().Be(1);
            product.Name.Should().Be(request.Name);
            product.ShortDescription.Should().Be(request.ShortDescription);
            product.LongDescription.Should().Be(request.LongDescription);
            product.Price.Should().Be(request.Price);
            product.OldPrice.Should().Be(request.OldPrice);
            product.ProductCategories.First().CategoryId.Should().Be(fakeCategory.Id);
            product.ProductCategories.First().IsFeaturedProduct.Should().BeFalse();


            //Assert.That(await TestHarness.Published.Any<CreateProductIntegrationEvent>());

        }


        private  Task<Category> CreateFakeCategory()
        {

            return  WithUnitOfWork(async (sp) =>
            {
                var fakeCategory = new Category("FakeCategory");
                var repository = sp.GetRequiredService<IRepository<Category>>();
                await  repository.InsertAsync(fakeCategory);

                return fakeCategory;
            });

        }

        private Task<int> GetProductsCount()
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();
                return repository.CountAsync();
            });
        }

        private Task<Product> GetProductById(Guid id)
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();
                return repository.SingleAsync(x=> x.Id == id);
            });
        }






    }
}
