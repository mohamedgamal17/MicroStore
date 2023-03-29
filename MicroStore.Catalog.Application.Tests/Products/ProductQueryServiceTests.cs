using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Products;
using System.Net;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class ProductQueryServiceTests : BaseTestFixture
    {
        private readonly IProductQueryService _productQueryService;
        public ProductQueryServiceTests()
        {
            _productQueryService= GetRequiredService<IProductQueryService>();
        }
        [Test]
        public async Task Should_get_product_list_paginated()
        {
            var queryParams = new PagingAndSortingQueryParams();
            var result = await _productQueryService.ListAsync(queryParams);
        
            
            result.IsSuccess.Should().BeTrue();
            result.Value.Skip.Should().Be(queryParams.Skip);
            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.Lenght);
        }



        [Test]
        public async Task Should_get_product_with_id()
        {
            string productId = "94174b5b-25a8-4c29-9364-3482e9356231";

            var result = await _productQueryService.GetAsync(productId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(productId);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_not_found_when_product_is_not_exist()
        {

            var response = await _productQueryService.GetAsync(Guid.NewGuid().ToString());

            response.IsFailure.Should().BeTrue();

        }

        [Test]
        public async Task Should_get_product_images()
        {
            string productId = "94174b5b-25a8-4c29-9364-3482e9356231";

            var result = await _productQueryService.ListProductImagesAsync(productId);

            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_product_images_when_product_id_is_not_exist()
        {
            var productId = Guid.NewGuid().ToString();

            var result = await _productQueryService.ListProductImagesAsync(productId);

            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }


    }
  
}
