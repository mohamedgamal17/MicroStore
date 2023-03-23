using FluentAssertions;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class ProductCommandServiceTests : ProductServiceTestBase
    {
        private readonly IProductCommandService _productCommandService;

        public ProductCommandServiceTests()
        {
            _productCommandService = GetRequiredService<IProductCommandService>();
        }

        [Test]
        public async Task Should_create_product()
        {
            var model = await GenerateProductModel();

            var result = await _productCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var product = await Find<Product>(x => x.Id == result.Value.Id);

            product.AssertProductModel(model);

        }

        [Test]
        public async Task Should_update_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var model = await GenerateProductModel();

            var result = await _productCommandService.UpdateAsync(fakeProduct.Id, model);

            result.IsSuccess.Should().BeTrue();

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.AssertProductModel(model);

            Assert.That(await TestHarness.Published.Any<ProductCreatedIntegrationEvent>());
        }
        [Test]
        public async Task Should_return_failure_result_while_updating_product_when_product_is_not_exist()
        {

            var model = await GenerateProductModel();

            var result = await _productCommandService.UpdateAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }
        [Test]
        public async Task Should_add_new_product_image()
        {
            var fakeProduct = await CreateFakeProduct();

            var model = new CreateProductImageModel
            {
                Image = Guid.NewGuid().ToString(),
                DisplayOrder = 1
            };

            var result = await _productCommandService.AddProductImageAsync(fakeProduct.Id, model);


            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            var productImage = product.ProductImages.Single(x => x.ImagePath == model.Image);

            productImage.ImagePath.Should().Be(model.Image);

            productImage.DisplayOrder.Should().Be(model.DisplayOrder);
        }

        [Test]
        public async Task Should_return_failure_result_while_adding_new_product_image_when_product_is_not_exist()
        {
            var model = new CreateProductImageModel
            {
                Image = Guid.NewGuid().ToString(),
                DisplayOrder = 1
            };

            var result = await _productCommandService.AddProductImageAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_update_product_image()
        {
            var fakeProduct = await CreateFakeProduct();

            var productImageId = fakeProduct.ProductImages.First().Id;

            var model = new UpdateProductImageModel
            {
                DisplayOrder = 5
            };

            var result = await _productCommandService.UpdateProductImageAsync(fakeProduct.Id, productImageId, model);


            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            var productImage = product.ProductImages.Single(x => x.Id == productImageId);

            productImage.DisplayOrder.Should().Be(model.DisplayOrder);
        }

        [Test]
        public async Task Should_return_failure_result_while_adding_updating_product_image_when_product_is_not_exist()
        {
            var model = new UpdateProductImageModel
            {
                DisplayOrder = 5
            };

            var result = await _productCommandService.UpdateProductImageAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),model);

            result.IsFailure.Should().BeTrue();


            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_updating_updating_product_image_when_product_image_is_not_exist_in_current_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var model = new UpdateProductImageModel
            {
                DisplayOrder = 5
            };

            var result = await _productCommandService.UpdateProductImageAsync(fakeProduct.Id, Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }



        [Test]
        public async Task Should_delete_product_image()
        {
            var fakeProduct = await CreateFakeProduct();

            var productImageId = fakeProduct.ProductImages.First().Id;

            var model = new UpdateProductImageModel
            {
                DisplayOrder = 5
            };

            var result = await _productCommandService.DeleteProductImageAsync(fakeProduct.Id, productImageId);


            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

            productImage.Should().BeNull();
        }

        [Test]
        public async Task Should_return_failure_result_while_adding_deleting_product_image_when_product_is_not_exist()
        {


            var result = await _productCommandService.DeleteProductImageAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_while_deleting_product_image_when_product_image_is_not_exist_in_current_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var model = new UpdateProductImageModel
            {
                DisplayOrder = 5
            };

            var result = await _productCommandService.DeleteProductImageAsync(fakeProduct.Id, Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }
    }

}
