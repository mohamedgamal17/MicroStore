using FluentAssertions;
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

            var response = await _categoryQueryService.ListAsync(new SortingQueryParams());

            response.IsSuccess.Should().BeTrue();

            response.Value.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Should_get_category_with_name()
        {
            string productId = "159b39f4-d03d-48df-9c89-ef5aaba7ef52";

            var response = await _categoryQueryService.GetAsync(productId);

            response.IsSuccess.Should().BeTrue();

            response.Value.Id.Should().Be(productId);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_category_is_not_exist()
        {
            var response = await _categoryQueryService.GetAsync(Guid.NewGuid().ToString());

            response.IsFailure.Should().BeTrue();

        }
    }


}
