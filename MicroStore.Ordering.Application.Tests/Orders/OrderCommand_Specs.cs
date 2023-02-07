
using FluentAssertions;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using MicroStore.Ordering.Application.StateMachines;
using System.Net;
using System.Reflection.PortableExecutable;

namespace MicroStore.Ordering.Application.Tests.Orders
{

    public class When_receiving_submit_order_command : OrderCommandTestBase
    {
        [Test]
        public async Task Should_submit_new_order()
        {

            var command = new SubmitOrderCommand
            {
                UserId = Guid.NewGuid().ToString(),
                BillingAddress = GenerateFakeAddress(),
                ShippingAddress = GenerateFakeAddress(),
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
    }


    public class When_receiving_cancel_order_command : OrderCommandTestBase
    {
        [Test]
        public async Task Should_cancel_order()
        {

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            CancelOrderCommand command = new CancelOrderCommand
            {
                OrderId = orderId,
                CancellationDate = DateTime.UtcNow,
                Reason = Guid.NewGuid().ToString()
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

            Assert.That(await TestHarness.Published.Any<OrderCancelledEvent>());

        }


        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_order_is_not_exist()
        {
            CancelOrderCommand command = new CancelOrderCommand
            {
                OrderId = Guid.NewGuid(),
                CancellationDate = DateTime.UtcNow,
                Reason = Guid.NewGuid().ToString()
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }



        [Test]
        public async Task Should_return_error_result_with_status_code_401_while_order_is_already_cancelled()
        {
            Guid orderId = Guid.NewGuid();

            await GenerateFakeCancelledOrder(orderId);

            CancelOrderCommand command = new CancelOrderCommand
            {
                OrderId = orderId,
                CancellationDate = DateTime.UtcNow,
                Reason = Guid.NewGuid().ToString()
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

    }


    public  class When_receiving_fullfill_order_command : OrderCommandTestBase
    {

        [Test]
        public async Task Should_fullfill_order()
        {
            Guid orderId = Guid.NewGuid();

            await GenerateFakeApprovedOrder(orderId);


            FullfillOrderCommand command = new FullfillOrderCommand
            {
                OrderId = orderId,
                ShipmentId = Guid.NewGuid().ToString(),
            };

            await Send(command);

            Assert.That(await TestHarness.Published.Any<OrderFulfillmentCompletedEvent>());

        }

        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_order_is_not_exist()
        {
            TestHarness.TestTimeout = TimeSpan.FromSeconds(1);

            FullfillOrderCommand command = new FullfillOrderCommand
            {
                OrderId = Guid.NewGuid(),
                ShipmentId = Guid.NewGuid().ToString(),
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_error_result_with_status_code_400_while_order_state_is_not_in_approved_state()
        {
            TestHarness.TestTimeout = TimeSpan.FromSeconds(1);

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            FullfillOrderCommand command = new FullfillOrderCommand
            {
                OrderId = orderId,
                ShipmentId = Guid.NewGuid().ToString(),

            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }

    public class When_receiving_complete_order_command : OrderCommandTestBase
    {
        [Test]
        public async Task Should_complete_order()
        {
            Guid orderId = Guid.NewGuid();

            await GenerateFakeFullfilledOrder(orderId);

            var command = new CompleteOrderCommand
            {
                OrderId = orderId,
                ShipedDate = DateTime.UtcNow,
            };

            var result = await Send(command);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

            Assert.That(await TestHarness.Published.Any<OrderCompletedEvent>());

        }


        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_is_not_exist()
        {

            CompleteOrderCommand command = new CompleteOrderCommand
            {
                OrderId = Guid.NewGuid(),
                ShipedDate = DateTime.UtcNow,
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        [Test]
        public async Task Should_return_error_result_with_status_code_401_while_order_state_is_not_in_fullfilled_state()
        {

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            CompleteOrderCommand command = new CompleteOrderCommand
            {
                OrderId = orderId,
                ShipedDate = DateTime.UtcNow,
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

    }
}
