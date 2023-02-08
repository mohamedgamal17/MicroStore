using FluentAssertions;
using MicroStore.Catalog.Application.Products;
using System.Net;
namespace MicroStore.Catalog.Application.Tests.Products
{
     
    public class When_receiving_get_product_list : BaseTestFixture
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

            var result = response.EnvelopeResult.Result;

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


    public class When_receiving_get_product_query : BaseTestFixture
    {

        [Test]
        public async Task Should_get_product_with_id()
        {
            var query = new GetProductQuery
            {
                Id = Guid.Parse("94174b5b-25a8-4c29-9364-3482e9356231"),

            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.Id);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_not_found_when_product_is_not_exist()
        {
            var query = new GetProductQuery
            {
                Id = Guid.NewGuid(),
            };

            var response = await Send(query);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
