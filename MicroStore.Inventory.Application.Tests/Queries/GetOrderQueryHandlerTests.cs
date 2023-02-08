using FluentAssertions;
using MicroStore.Inventory.Application.Orders;
using System.Net;
namespace MicroStore.Inventory.Application.Tests.Queries
{
    public class GetOrderQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_order_with_id()
        {
            var query = new GetOrderQuery
            {
                OrderId = Guid.Parse("159b39f4-d03d-48df-9c89-ef5aaba7ef52")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.OrderId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_order_id_is_not_exist()
        {
            var query = new GetOrderQuery
            {
                OrderId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
