using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.PaymentRequests;
namespace MicroStore.Payment.Application.Tests.PaymentRequests
{

    public class PaymentRequestQueryServiceTests : BaseTestFixture
    {
        private readonly IPaymentRequestQueryService _paymentRequestQueryService;

        public PaymentRequestQueryServiceTests()
        {
            _paymentRequestQueryService = GetRequiredService<IPaymentRequestQueryService>();
        }

        [Test]
        public async Task Should_get_payment_request_list_paged()
        {
            var queryParams = new PagingAndSortingQueryParams { PageNumber = 1, PageSize = 3 };

            var result = await _paymentRequestQueryService.ListPaymentAsync(queryParams);
            result.IsSuccess.Should().BeTrue();

            result.Value.PageNumber.Should().Be(queryParams.PageNumber);
            result.Value.PageSize.Should().Be(queryParams.PageSize);

            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);

        }

        [Test]
        public async Task Should_get_user_payment_request_list_paged()
        {
            string userId = "2cd94e7f-d80a-41c9-9805-75f1e3b4b925";

            var queryParams = new PagingAndSortingQueryParams { PageNumber = 1, PageSize = 3 };

            var result = await _paymentRequestQueryService.ListPaymentAsync(queryParams, userId);

            result.IsSuccess.Should().BeTrue();

      

            result.Value.PageNumber.Should().Be(queryParams.PageNumber);
            result.Value.PageSize.Should().Be(queryParams.PageSize);

            result.Value.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);

            result.Value.Items.All(x => x.UserId == userId).Should().BeTrue();
        }

        [Test]
        public async Task Should_get_payment_request_with_id()
        {
            string paymentId = "dd2be1f2-e980-40f2-a47d-b9194ef03fe7";

            var result = await _paymentRequestQueryService.GetAsync(paymentId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(paymentId);
        }

        [Test]
        public async Task Should_return_error_result_while_get_payment_request_by_id_when_payment_request_is_not_exist()
        {
            var result = await _paymentRequestQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }


        [Test]
        public async Task Should_get_payment_request_with_order_id()
        {
            string orderId = "b50ca3ac-7065-4cd8-9b89-b764aa71e444";

            var result = await _paymentRequestQueryService.GetByOrderIdAsync(orderId);

            result.IsSuccess.Should().BeTrue();

            result.Value.OrderId.Should().Be(orderId);
        }

        [Test]
        public async Task Should_return_error_result_while_get_payment_request_by_order_id_when_payment_id_request_is_not_exist()
        {
            var result = await _paymentRequestQueryService.GetByOrderIdAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }


        [Test]
        public async Task Should_get_payment_request_with_order_number()
        {
            string orderNumber = "6820be8e-0f4e-4ae2-94dc-e226a0e8f2f7";

            var result = await _paymentRequestQueryService.GetByOrderNumberAsync(orderNumber);

            result.IsSuccess.Should().BeTrue();

            result.Value.OrderNumber.Should().Be(orderNumber);
        }

        [Test]
        public async Task Should_return_error_result_while_get_payment_request_by_order_number_when_payment_id_request_is_not_exist()
        {
            var result = await _paymentRequestQueryService.GetByOrderIdAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);

        }
    } 
}
