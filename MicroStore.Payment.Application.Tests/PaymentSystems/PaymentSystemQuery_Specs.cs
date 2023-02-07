using FluentAssertions;
using MicroStore.Payment.Application.PaymentSystems;
using System.Net;

namespace MicroStore.Payment.Application.Tests.PaymentSystems
{
    public class When_receiving_get_payment_system_list_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_system_list()
        {
            var query = new GetPaymentSystemListQuery();

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Items.Count.Should().BeGreaterThan(0);

        }
    }


    public class When_receiving_get_payment_system_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_system_with_id()
        {
            var query = new GetPaymentSystemQuery
            {
                SystemId = Guid.Parse("6cc93286-de57-4fe8-af64-90bdbe378e40")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.SystemId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_payment_system_id_is_not_exist()
        {
            var query = new GetPaymentSystemQuery
            {
                SystemId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }

    public class When_receiving_get_payment_request_with_name_query : BaseTestFixture
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
