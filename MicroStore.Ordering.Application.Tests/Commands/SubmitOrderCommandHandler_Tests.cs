using FluentAssertions;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.StateMachines;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.IntegrationEvents;
using System.Net;

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
                BillingAddress = GenerateFakeAddress(),
                ShippingAddress  = GenerateFakeAddress(),
                ShippingCost = 0,
                TaxCost = 0,
                SubTotal = 50,
                TotalPrice = 100,
                SubmissionDate = DateTime.Now,
                OrderItems = new List<OrderItemModel>
                {
                     new OrderItemModel
                     { 
                          ExternalProductId = Guid.NewGuid().ToString(),
                          Name = Guid.NewGuid().ToString(),
                          Sku = Guid.NewGuid().ToString(),                     
                          Quantity = 5,
                          UnitPrice = 50
                     }
                }
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

            Assert.That(await TestHarness.Published.Any<OrderSubmitedEvent>());
           
        }

        private AddressModel GenerateFakeAddress()
        {
            return new AddressModel
            {
                CountryCode = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                AddressLine1 = Guid.NewGuid().ToString(),
                AddressLine2 = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString(),
                Zip = Guid.NewGuid().ToString(),
            };
        }


     

    }
}
