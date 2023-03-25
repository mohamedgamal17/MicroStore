using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Products;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Net;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Inventory.Application.Tests.Products
{
    public class ProductQueryServiceTests : BaseTestFixture
    {
        private readonly IProductQueryService _productQueryService;

        public ProductQueryServiceTests()
        {
            _productQueryService = GetRequiredService<IProductQueryService>();  

        }

        [Test]
        public async Task Should_get_product_list_paged()
        {
            var queryParams = new PagingQueryParams();


            var result = await _productQueryService.ListAsync(queryParams);

            result.IsSuccess.Should().BeTrue();

            result.Value.Skip.Should().Be(queryParams.Skip);

            result.Value.Lenght.Should().Be(queryParams.Lenght);

            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.Lenght);
        }


        [Test]
        public async Task Should_get_product_with_id()
        {
            var productId = (await FirstAsync<Product>()).Id;

            var result = await _productQueryService.GetAsync(productId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(productId);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_product_by_id_while_product_is_not_exist()
        {

            var result = await _productQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_get_product_with_sku()
        {
            var sku = "T-ShirtXMas";

            var result = await _productQueryService.GetBySkyAsync(sku);

            result.IsSuccess.Should().BeTrue();

            result.Value.Sku.Should().Be(sku);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_product_by_sku_while_product_is_not_exist()
        {

            var result = await _productQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

    }
}
