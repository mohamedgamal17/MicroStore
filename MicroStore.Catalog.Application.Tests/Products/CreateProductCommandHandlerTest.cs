using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Models;
using MicroStore.Catalog.Application.Tests.Utilites;
using MicroStore.Catalog.Domain.Entities;
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
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                ImageModel = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}.png",
                    Type = "png",
                    Data = ImageGenerator.GetBitmapData()
                },
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 50,
                OldPrice = 150,

            };

            var result = await Send(request);


            var productsConut = await GetProductsCount();

            var product = await GetProductById(result.ProductId);

            productsConut.Should().Be(1);
            product.Name.Should().Be(request.Name);
            product.ShortDescription.Should().Be(request.ShortDescription);
            product.LongDescription.Should().Be(request.LongDescription);
            product.Price.Should().Be(request.Price);
            product.OldPrice.Should().Be(request.OldPrice);
            product.Thumbnail.Should().EndWith(request.ImageModel.FileName);
        }


        private Task<Category> CreateFakeCategory()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var fakeCategory = new Category(Guid.NewGuid().ToString());
                var repository = sp.GetRequiredService<IRepository<Category>>();
                await repository.InsertAsync(fakeCategory);
                return fakeCategory;
            });        
        }

        private Task<int> GetProductsCount()
        {
            return WithUnitOfWork(sp =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.CountAsync();
            });
        }

        private Task<Product> GetProductById(Guid id)
        {
            return WithUnitOfWork(sp =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.SingleAsync(x => x.Id == id);
            });
        }

    }
}
