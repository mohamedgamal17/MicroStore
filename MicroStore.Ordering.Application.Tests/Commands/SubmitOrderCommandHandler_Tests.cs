using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.Commands
{
    public class SubmitOrderCommandHandler_Tests : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {

        [Test]
        public async Task Should_submit_new_order()
        {

            var command = new SubmitOrderCommand
            {
                UserId = Guid.NewGuid().ToString(),
                BillingAddressId = Guid.NewGuid(),
                ShippingAddressId = Guid.NewGuid(),
                ShippingCost = 0,
                TaxCost = 0,
                SubTotal = 50,
                TotalPrice = 100,
                SubmissionDate = DateTime.Now,
                OrderItems = new List<OrderItemModel>
                {
                     new OrderItemModel
                     {
                          ItemName = Guid.NewGuid().ToString(),
                          ProductId = Guid.NewGuid(),
                          Quantity = 5,
                          UnitPrice = 50
                     }
                }
            };

            var response = await Send(command);

            Assert.That(await TestHarness.Published.Any<SubmitOrderIntegrationEvent>());
           
        }


     

    }
}
