using FluentAssertions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Inventory.Application.Queries.Tests.Queries
{
    public class GetProductListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_product_list_paged()
        {
            var query = new GetProductListQuery
            {
                PageSize = 4,
                PageNumber = 1
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<PagedResult<ProductDto>>().Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }

    }
}
