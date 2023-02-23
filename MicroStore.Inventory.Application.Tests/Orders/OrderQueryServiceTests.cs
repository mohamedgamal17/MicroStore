using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.Domain.OrderAggregate;
using System.Net;

namespace MicroStore.Inventory.Application.Tests.Orders
{
    public class OrderQueryServiceTests : BaseTestFixture
    {
        private readonly IOrderQueryService _orderQuerService;


        public OrderQueryServiceTests()
        {
            _orderQuerService = GetRequiredService<IOrderQueryService>();
        }

        [Test]
        public async Task Should_get_order_paged_list()
        {
            var queryParams = new PagingQueryParams
            {
                PageNumber = 1,
                PageSize = 3
            };

            var result = await _orderQuerService.ListOrderAsync(queryParams);

            result.IsSuccess.Should().BeTrue();

            result.Value.PageNumber.Should().Be(queryParams.PageNumber);
            result.Value.PageSize.Should().Be(queryParams.PageSize);

            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);
        }


        [Test]
        public async Task Should_get_user_order_list_paged()
        {
            string userId = "2cd94e7f-d80a-41c9-9805-75f1e3b4b925";

            var queryParams = new PagingQueryParams
            {
                PageNumber = 1,
                PageSize = 3,
            };


            var result = await _orderQuerService.ListOrderAsync(queryParams,userId);

            result.IsSuccess.Should().BeTrue();

            result.Value.PageNumber.Should().Be(queryParams.PageNumber);
            result.Value.PageSize.Should().Be(queryParams.PageSize);

            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);
        }


        [Test]
        public async Task Should_get_order_with_id()
        {
            string orderId = (await FirstAsync<Order>()).Id;


            var result = await _orderQuerService.GetOrderAsync(orderId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(orderId);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_order_by_id_when_order_id_is_not_exist()
        {

            var result = await _orderQuerService.GetOrderAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        [Test]
        public async Task Should_get_order_with_order_number()
        {
            string orderNumber = "6820be8e-0f4e-4ae2-94dc-e226a0e8f2f7";


            var result = await _orderQuerService.GetOrderByNumberAsync(orderNumber);

            result.IsSuccess.Should().BeTrue();

            result.Value.OrderNumber.Should().Be(orderNumber);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_order_by_order_number_when_order_number_is_not_exist()
        {

            var result = await _orderQuerService.GetOrderByNumberAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }
    }
}
