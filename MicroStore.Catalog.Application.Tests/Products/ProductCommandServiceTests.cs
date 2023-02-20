using FluentAssertions;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
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

            var product = await Find<Product>(x => x.Id == result.Result.Id);

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
    }

}
