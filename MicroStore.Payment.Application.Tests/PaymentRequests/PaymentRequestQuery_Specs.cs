using FluentAssertions;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Application.PaymentSystems;
using System.Net;
namespace MicroStore.Payment.Application.Tests.PaymentRequests
{
   
    public class When_receiving_get_payment_request_list_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_request_list_paged()
        {
            var query = new GetPaymentRequestListQuery
            {
                PageNumber = 1,
                PageSize = 3
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task Should_get_payment_request_list_paged_sorted_by_creation_date(bool desc)
        {
            var query = new GetPaymentRequestListQuery
            {
                PageNumber = 1,
                PageSize = 3,
                SortBy = "creation",
                Desc = desc
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            if (desc)
            {
                result.Items.Should().BeInDescendingOrder(x => x.CreationTime);
            }
            else
            {
                result.Items.Should().BeInAscendingOrder(x => x.CreationTime);
            }
        }

    }

    public class When_receiving_get_user_payment_request_list : BaseTestFixture
    {
        [Test]
        public async Task Should_get_user_payment_request_list_paged()
        {
            var query = new GetUserPaymentRequestListQuery
            {
                PageNumber = 1,
                PageSize = 3,
                UserId = "2cd94e7f-d80a-41c9-9805-75f1e3b4b925"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);

            result.Items.All(x => x.CustomerId == query.UserId).Should().BeTrue();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task Should_get_user_payment_request_list_sorted_by_creation_date(bool desc)
        {
            var query = new GetUserPaymentRequestListQuery
            {
                PageNumber = 1,
                PageSize = 3,
                UserId = "2cd94e7f-d80a-41c9-9805-75f1e3b4b925",
                SortBy = "creation",
                Desc = desc
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            if (desc)
            {
                result.Items.Should().BeInDescendingOrder(x => x.CreationTime);
            }
            else
            {
                result.Items.Should().BeInAscendingOrder(x => x.CreationTime);
            }
        }
    }

    public class When_receiving_get_payment_request_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_request_with_id()
        {
            var query = new GetPaymentRequestQuery
            {
                PaymentRequestId = Guid.Parse("dd2be1f2-e980-40f2-a47d-b9194ef03fe7")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.PaymentRequestId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_payment_id_request_is_not_exist()
        {
            var query = new GetPaymentRequestQuery
            {
                PaymentRequestId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }
    }

    public class When_receving_get_payment_request_with_order_id_query : BaseTestFixture
    {

        [Test]
        public async Task Should_get_payment_system_with_name()
        {
            var query = new GetPaymentSystemWithNameQuery
            {
                SystemName = "Example"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Name.Should().Be(query.SystemName);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_payment_system_name_is_not_exist()
        {
            var query = new GetPaymentSystemWithNameQuery
            {
                SystemName = Guid.NewGuid().ToString()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
