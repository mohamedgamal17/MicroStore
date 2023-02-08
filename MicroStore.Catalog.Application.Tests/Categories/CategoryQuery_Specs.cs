using FluentAssertions;
using MicroStore.Catalog.Application.Categories;
using System.Net;

namespace MicroStore.Catalog.Application.Tests.Categories
{
    public class When_receiving_get_category_list_query : BaseTestFixture
    {

        [Test]
        public async Task Should_get_all_categories()
        {
            var query = new GetCategoryListQuery();

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Items.Count.Should().BeGreaterThan(0);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Should_get_categories_sorted_according_to_name(bool desc)
        {
            var query = new GetCategoryListQuery()
            {
                SortBy = "name",
                Desc = desc
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);


            var result = response.EnvelopeResult.Result;

            if (desc)
            {
                result.Items.Should().BeInDescendingOrder(x => x.Name);
            }
            else
            {
                result.Items.Should().BeInAscendingOrder(x => x.Name);
            }
        }
    }

    public class When_receiving_get_category_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_category_with_name()
        {
            var query = new GetCategoryQuery
            {
                Id = Guid.Parse("159b39f4-d03d-48df-9c89-ef5aaba7ef52")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.Id);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_when_category_is_not_exist()
        {
            var query = new GetCategoryQuery
            {
                Id = Guid.NewGuid()
            };

            var response = await Send(query);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
