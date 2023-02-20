using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Products;
using System.Net;
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
            var queryParams = new PagingAndSortingQueryParams
            {
                PageNumber = 1,
                PageSize = 5,
            };

            var result = await _productQueryService.ListAsync(queryParams);
        
            
            result.IsSuccess.Should().BeTrue();
            result.IsSuccess.Should().BeTrue();
            result.Result.PageNumber.Should().Be(queryParams.PageNumber);
            result.Result.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);
        }



        [Test]
        public async Task Should_get_product_with_id()
        {
            string productId = "94174b5b-25a8-4c29-9364-3482e9356231";

            var result = await _productQueryService.GetAsync(productId);

            result.IsSuccess.Should().BeTrue();

            result.Result.Id.Should().Be(productId);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_not_found_when_product_is_not_exist()
        {

            var response = await _productQueryService.GetAsync(Guid.NewGuid().ToString());

            response.IsFailure.Should().BeTrue();

        }


    }
  
}
