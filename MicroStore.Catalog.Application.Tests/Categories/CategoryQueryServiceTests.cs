﻿using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Categories;
namespace MicroStore.Catalog.Application.Tests.Categories
{

    public class CategoryQueryServiceTests : BaseTestFixture
    {
        private readonly ICategoryQueryService _categoryQueryService;
        public CategoryQueryServiceTests()
        {
            _categoryQueryService= GetRequiredService<ICategoryQueryService>();
        }

        [Test]
        public async Task Should_get_all_categories()
        {
            var queryParams = new PagingAndSortingQueryParams();

            var result = await _categoryQueryService.ListAsync(queryParams);

            result.IsSuccess.Should().BeTrue();


            result.IsSuccess.Should().BeTrue();
            result.Value.Skip.Should().Be(queryParams.Skip);
            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.Lenght);
        }

        [Test]
        public async Task Should_get_category_with_name()
        {
            string productId = "159b39f4-d03d-48df-9c89-ef5aaba7ef52";

            var result = await _categoryQueryService.GetAsync(productId);

            result.IsSuccess.Should().BeTrue();

            result.IfSuccess((val) =>
            {
                val.Id.Should().Be(productId);
            });

        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_category_is_not_exist()
        {
            var response = await _categoryQueryService.GetAsync(Guid.NewGuid().ToString());

            response.IsFailure.Should().BeTrue();

        }
    }


}
