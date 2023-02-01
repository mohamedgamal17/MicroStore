﻿using MicroStore.Ordering.Application.Abstractions.Queries;
using FluentAssertions;
using System.Net;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Ordering.Application.Abstractions.Dtos;

namespace MicroStore.Ordering.Application.Tests.Queries
{
    public class GeOrderListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_order_paged_list()
        {
            var query = new GetOrderListQuery
            {
                PageNumber = 1,
                PageSize = 10,
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);

        }
    }
}