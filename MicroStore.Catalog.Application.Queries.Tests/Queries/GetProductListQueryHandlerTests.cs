using FluentAssertions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using System.Net;

namespace MicroStore.Catalog.Application.Queries.Tests.Queries
{
    public class GetProductListQueryHandlerTests : BaseTestFixture
    {

        [Test]
        public async Task Should_get_product_list_paginated()
        {
            var query = new GetProductListQuery
            {
                PageNumber = 1,
                PageSize = 5,
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<PagedResult<ProductListDto>>().Result;

            result.PageSize.Should().Be(query.PageSize);
            result.PageNumber.Should().Be(query.PageNumber);
            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task Should_get_product_list_sorted_according_to_name(bool desc)
        {
            var query = new GetProductListQuery
            {
                PageNumber = 1,
                PageSize = 5,
                SortBy = "name",
                Desc = desc
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<PagedResult<ProductListDto>>().Result;

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
