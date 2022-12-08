using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Tests.Utilites;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class UpdateProductCommandHandlerTest : BaseTestFixture
    {

        [Test]
        public async Task Should_update_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new UpdateProductCommand
            {
                ProductId = fakeProduct.Id,
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 120,
                OldPrice = 150,
                Weight = new WeightModel
                {
                    Value = 50,
                    Unit = "g"
                },
                Length = new DimensionModel
                {
                    Value = 50,
                    Unit = "cm",
                },

                Width = new DimensionModel
                {
                    Value = 50,
                    Unit = "cm",
                },

                Height = new DimensionModel
                {
                    Value = 50,
                    Unit = "cm",
                },
            };


            await Send(command);

            var product = await Find<Product>(x=> x.Id == fakeProduct.Id);


            product.Sku.Should().Be(command.Sku);
            product.Name.Should().Be(command.Name);
            product.ShortDescription.Should().Be(command.ShortDescription);
            product.LongDescription.Should().Be(command.LongDescription);
            product.Price.Should().Be(command.Price);
            product.OldPrice.Should().Be(command.OldPrice);
            product.Weight.Should().Be(command.Weight.AsWeight());
            product.Height.Should().Be(command.Height.AsDimension());
            product.Length.Should().Be(command.Length.AsDimension());
            product.Width.Should().Be(command.Width.AsDimension());
        }

        [Test]
        public async Task Should_update_product_thumbnail_while_image_model_is_not_null()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new UpdateProductCommand
            {
                ProductId = fakeProduct.Id,
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 120,
                OldPrice = 150,

                ImageModel =  new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}.png",
                    Type = "png",
                    Data = ImageGenerator.GetBitmapData()
                },
            };

            await Send(command);

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.Thumbnail.Should().EndWith(command.ImageModel.FileName);

        }


        [Test]
        public async Task Should_throw_entity_not_found_exception_while_product_is_not_exist()
        {
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 120,
                OldPrice = 150,
            };

            Func<Task> func = ()=> Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }



        private  Task<Product> CreateFakeProduct()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var fakeProduct = new Product("FakeSku", "FakeName", 50, Guid.NewGuid().ToString());
                var repository = sp.GetRequiredService<IRepository<Product>>();
                await repository.InsertAsync(fakeProduct);
                return fakeProduct;
            });
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

    }
}
