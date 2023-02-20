using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Orders;
namespace MicroStore.Ordering.Application.Tests.Orders
{

    public class OrderQueryServiceTests : BaseTestFixture 
    {
        private readonly IOrderQueryService _orderQueryService;

        public OrderQueryServiceTests()
        {
            _orderQueryService= GetRequiredService<IOrderQueryService>();
        }
        [Test]
        public async Task Should_get_order_paged_list()
        {
            var queryParams = new PagingAndSortingQueryParams
            {
                PageNumber = 1,
                PageSize = 10,
            };

            var response = await _orderQueryService.ListAsync(queryParams);

            response.IsSuccess.Should().BeTrue();

            var result = response.Result;

            result.PageNumber.Should().Be(queryParams.PageNumber);
            result.PageSize.Should().Be(queryParams.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);

        }

        [Test]
        public async Task Should_get_user_order_list_paged()
        {
            string userId = "a84eb974-50b7-4ce6-95ac-515b54d2d7a3";

            var queryParams = new PagingAndSortingQueryParams
            {
                
                PageNumber = 1,
                PageSize = 10
            };

            var response = await _orderQueryService.ListAsync(queryParams, userId);

            response.IsSuccess.Should().BeTrue();

            var result = response.Result;

            result.PageNumber.Should().Be(queryParams.PageNumber);

            result.PageSize.Should().Be(queryParams.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);

            result.Items.All(x => x.UserId == userId).Should().BeTrue();
        }
        [Test]
        public async Task Should_get_order_with_id()
        {
            var orderId = Guid.Parse("5abe7f28-9e3c-40f5-a0cc-1ba1bffed3f9");

            var response = await _orderQueryService.GetAsync(orderId);

            response.IsSuccess.Should().BeTrue();

            response.Result.Id.Should().Be(orderId);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_order_by_id_when_order_is_not_exist()
        {

            var response = await _orderQueryService.GetAsync(Guid.NewGuid());

            response.IsFailure.Should().BeTrue();
        }


        [Test]
        public async Task Should_get_order_by_number()
        {
            var orderNumber = "6820be8e-0f4e-4ae2-94dc-e226a0e8f2f7";

            var response = await _orderQueryService.GetByOrderNumberAsync(orderNumber);

            response.IsSuccess.Should().BeTrue();

            response.Result.OrderNumber.Should().Be(orderNumber);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_order_by_number_when_order_is_not_exist()
        {

            var response = await _orderQueryService.GetByOrderNumberAsync(Guid.NewGuid().ToString());

            response.IsFailure.Should().BeTrue();
        }

    }

   

}
