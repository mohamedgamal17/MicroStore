using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using FluentAssertions;
using System.Net;

namespace MicroStore.Catalog.Application.Tests.Categories.Queries
{
    public class GetCategoryQueryHandlerTests : BaseTestFixture
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
