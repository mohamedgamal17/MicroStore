using FluentAssertions;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Tests.Categories.Queries
{
    public class GetCategoryListQueryHandlerTests : BaseTestFixture
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
}
