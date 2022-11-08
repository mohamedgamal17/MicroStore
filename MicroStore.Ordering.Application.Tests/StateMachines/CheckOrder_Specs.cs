using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.Events;
using Volo.Abp.Users;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace MicroStore.Ordering.Application.Tests.StateMachines
{
    public class When_check_order_request_recived : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {


        private readonly ICurrentUser _currentUser;

        public When_check_order_request_recived()
        {
            _currentUser = ServiceProvider.GetRequiredService<ICurrentUser>();
        }


        [Test]
        public async Task Should_responed_with_order_response()
        {
            Guid fakeOrderId = Guid.NewGuid();

            string fakeOrderNumber = Guid.NewGuid().ToString();

            await TestHarness.Bus.Publish(
                  new OrderSubmitedEvent
                  {
                      OrderId = fakeOrderId,
                      OrderNumber = fakeOrderNumber,
                      BillingAddressId = Guid.NewGuid(),
                      ShippingAddressId = Guid.NewGuid(),
                      UserId = _currentUser.Id!.ToString(),
                      SubmissionDate = DateTime.UtcNow,
                      OrderItems = GenerateFakeOrderItems()
                  }
              );


            var requestClinet = TestHarness.GetRequestClient<CheckOrderRequest>();


            var response = await requestClinet.GetResponse<OrderResponse>(new CheckOrderRequest
            {
                OrderId = fakeOrderId
            });

            response.Message.OrderId.Should().Be(fakeOrderId);
        }


        [Test]
        public async Task Should_responed_with_order_not_found_when_order_is_not_exist()
        {
            Guid fakeOrderId = Guid.NewGuid();

            var requestClinet = TestHarness.GetRequestClient<CheckOrderRequest>();


            var response = await requestClinet.GetResponse<OrderNotFound>(new CheckOrderRequest
            {
                OrderId = fakeOrderId
            });

            response.Message.OrderId.Should().Be(fakeOrderId);
        }
        private List<OrderItemModel> GenerateFakeOrderItems()
        {
            return new List<OrderItemModel>
            {

                 new OrderItemModel
                 {
                      ItemName = "FakeName",
                       ProductId = Guid.NewGuid(),
                       Quantity = 5,
                       UnitPrice = 50
                 }
            };
        }

    }
}
