using FluentAssertions;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Application.Operations;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.IntegrationEvents;
using Volo.Abp.Domain.Entities;
using MicroStore.Catalog.Application.Abstractions.Products;
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

            await result.IfSuccess(async val =>
            {
                var product = await SingleAsync<Product>(x => x.Id == val.Id);

                product.AssertProductModel(model);

                Assert.That(await TestHarness.Published.Any<ProductCreatedIntegrationEvent>());

                Assert.That(await TestHarness.Published.Any<EntityCreatedEvent<ProductEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityCreatedEvent<ProductEto>>());

                var elasticProduct = await FindElasticDoc<ElasticProduct>(val.Id);

                elasticProduct.Should().NotBeNull();

                elasticProduct!.AssertElasticProduct(product);
            });
        }

        [Test]
        public async Task Should_update_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var model = await GenerateProductModel();

            var result = await _productCommandService.UpdateAsync(fakeProduct.Id, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async val =>
            {

                var product = await SingleAsync<Product>(x => x.Id == val.Id);

                product.AssertProductModel(model);

                Assert.That(await TestHarness.Published.Any<ProductUpdatedIntegerationEvent>());

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ProductEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ProductEto>>());

                var elasticProduct = await FindElasticDoc<ElasticProduct>(result.Value.Id);

                elasticProduct.Should().NotBeNull();

                elasticProduct!.AssertElasticProduct(product);
            });

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

            var model = new ProductImageModel
            {
                Image = "https://developers.google.com/nest/device-access/images/device-camera.png",
                DisplayOrder = 1
            };

            var result = await _productCommandService.AddProductImageAsync(fakeProduct.Id, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {

                var product = await SingleAsync<Product>(x => x.Id == fakeProduct.Id);

                var productImage = product.ProductImages.Single(x => x.Image == model.Image);

                productImage.Image.Should().Be(model.Image);

                productImage.DisplayOrder.Should().Be(model.DisplayOrder);

                Assert.That(await TestHarness.Published.Any<ProductUpdatedIntegerationEvent>());

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ProductEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ProductEto>>());
            });

        }

        [Test]
        public async Task Should_return_failure_result_while_adding_new_product_image_when_product_is_not_exist()
        {
            var model = new ProductImageModel
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

            var model = new ProductImageModel
            {
                Image = "https://developers.google.com/nest/device-access/images/device-camera.png",
                DisplayOrder = 5
            };

            var result = await _productCommandService.UpdateProductImageAsync(fakeProduct.Id, productImageId, model);

            await result.IfSuccess(async _ =>
            {
                var product = await SingleAsync<Product>(x => x.Id == fakeProduct.Id);

                var productImage = product.ProductImages.Single(x => x.Id == productImageId);

                productImage.Image.Should().Be(model.Image);

                productImage.DisplayOrder.Should().Be(model.DisplayOrder);

                Assert.That(await TestHarness.Published.Any<ProductUpdatedIntegerationEvent>());

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ProductEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ProductEto>>());
            });

        }

        [Test]
        public async Task Should_return_failure_result_while_adding_updating_product_image_when_product_is_not_exist()
        {
            var model = new ProductImageModel
            {
                Image = "https://developers.google.com/nest/device-access/images/device-camera.png",
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

            var model = new ProductImageModel
            {
                Image = "https://developers.google.com/nest/device-access/images/device-camera.png",
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

            var model = new ProductImageModel
            {
                Image = "https://developers.google.com/nest/device-access/images/device-camera.png",
                DisplayOrder = 5
            };

            var result = await _productCommandService.DeleteProductImageAsync(fakeProduct.Id, productImageId);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var product = await SingleAsync<Product>(x => x.Id == fakeProduct.Id);

                var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

                productImage.Should().BeNull();

                Assert.That(await TestHarness.Published.Any<ProductUpdatedIntegerationEvent>());

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ProductEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ProductEto>>());
            });


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

            var result = await _productCommandService.DeleteProductImageAsync(fakeProduct.Id, Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_create_product_specification_attribute()
        {

            var fakeProduct = await CreateFakeProduct();

            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var model = new ProductSpecificationAttributeModel
            {
                AttributeId = fakeAttribute.Id,
                OptionId = fakeAttribute.Options.First().Id
            };

            var result = await _productCommandService.CreateProductAttributeSpecificationAsync(fakeProduct.Id, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var prodcut = await SingleAsync<Product>(x => x.Id == fakeProduct.Id);

                var productSpecificationAttribute = prodcut.SpecificationAttributes.SingleOrDefault(x => x.AttributeId == model.AttributeId && x.OptionId == model.OptionId);

                productSpecificationAttribute.Should().NotBeNull();

                Assert.That(await TestHarness.Published.Any<ProductUpdatedIntegerationEvent>());

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ProductEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ProductEto>>());
            });

  
        }


        [Test]
        public async Task Should_return_failure_result_when_create_product_specification_attribute_when_product_not_exist()
        {

            var productId = Guid.NewGuid().ToString();

            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var model = new ProductSpecificationAttributeModel
            {
                AttributeId = fakeAttribute.Id,
                OptionId = fakeAttribute.Options.First().Id
            };

            var result = await _productCommandService.CreateProductAttributeSpecificationAsync(productId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_remove_product_specification_attribute()
        {
            var fakeProduct = await CreateFakeProduct();

            var productSpecificationAttributeId = fakeProduct.SpecificationAttributes.First().Id;

            var result = await _productCommandService.RemoveProductAttributeSpecificationAsync(fakeProduct.Id, productSpecificationAttributeId);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var product = await SingleAsync<Product>(x => x.Id == fakeProduct.Id);

                var productSpecificationAttribute = product.SpecificationAttributes.SingleOrDefault(x => x.Id == productSpecificationAttributeId);

                productSpecificationAttribute.Should().BeNull();

                Assert.That(await TestHarness.Published.Any<ProductUpdatedIntegerationEvent>());

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ProductEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ProductEto>>());
            });
   
        }

        [Test]
        public async  Task Should_return_failure_result_when_removing_product_specification_while_product_is_not_exist()
        {
            var productId = Guid.NewGuid().ToString();

            var productSpecificationAttributeId = Guid.NewGuid().ToString();

            var result = await _productCommandService.RemoveProductAttributeSpecificationAsync(productId, productSpecificationAttributeId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_when_removing_product_specification_while_product_specification_is_not_exist()
        {
            var fakeProduct = await CreateFakeProduct();

            var productSpecificationAttributeId = Guid.NewGuid().ToString();

            var result = await _productCommandService.RemoveProductAttributeSpecificationAsync(fakeProduct.Id, productSpecificationAttributeId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

    }

}
