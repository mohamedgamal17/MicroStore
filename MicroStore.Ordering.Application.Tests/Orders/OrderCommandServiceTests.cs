using FluentAssertions;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using MicroStore.Ordering.Application.StateMachines;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Ordering.Application.Tests.Orders
{

    public class OrderCommandServiceTests : OrderCommandTestBase
    {
        private readonly IOrderCommandService _orderCommandService;

        public OrderCommandServiceTests()
        {
            _orderCommandService = GetRequiredService<IOrderCommandService>();
        }

        [Test]
        public async Task Should_submit_new_order()
        {
            var model = new CreateOrderModel
            {
                UserId = Guid.NewGuid().ToString(),
                BillingAddress = GenerateFakeAddress(),
                ShippingAddress = GenerateFakeAddress(),
                ShippingCost = 0,
                TaxCost = 0,
                SubTotal = 50,
                TotalPrice = 100,
                Items = new List<OrderItemModel>
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

            var result = await _orderCommandService.CreateOrderAsync(model);

            result.IsSuccess.Should().BeTrue();

            Assert.That(await TestHarness.Published.Any<OrderSubmitedEvent>());

        }
        [Test]
        public async Task Should_fullfill_order()
        {
            Guid orderId = Guid.NewGuid();

            await GenerateFakeApprovedOrder(orderId);


            var model = new FullfillOrderModel
            {
                ShipmentId = Guid.NewGuid().ToString(),
            };

            var result = await _orderCommandService.FullfillOrderAsync(orderId,model);

            Assert.That(await TestHarness.Published.Any<OrderFulfillmentCompletedEvent>());

        }

        [Test]
        public async Task Should_return_error_result_while_fullfilling_order_when_order_is_not_exist()
        {

            var model = new FullfillOrderModel
            {
                ShipmentId = Guid.NewGuid().ToString(),
            };
            var result = await _orderCommandService.FullfillOrderAsync(Guid.NewGuid(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_error_result_while_fullfilling_order_when_order_state_is_not_in_approved_state()
        {

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            var model = new FullfillOrderModel
            {
                ShipmentId = Guid.NewGuid().ToString(),
            };

            var result = await _orderCommandService.FullfillOrderAsync(orderId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<BusinessException>();

        }


        [Test]
        public async Task Should_complete_order()
        {
            Guid orderId = Guid.NewGuid();

            await GenerateFakeFullfilledOrder(orderId);

            var result = await _orderCommandService.CompleteOrderAsync(orderId);

            result.IsSuccess.Should().BeTrue();

            Assert.That(await TestHarness.Published.Any<OrderCompletedEvent>());

        }


        [Test]
        public async Task Should_return_error_result_while_completing_order_code_404_while_is_not_exist()
        {
            var result = await _orderCommandService.CompleteOrderAsync(Guid.NewGuid());

            result.IsFailure.Should().BeTrue();


            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_return_error_result_while_completing_order_when_order_state_is_not_in_fullfilled_state()
        {

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            var result = await _orderCommandService.CompleteOrderAsync(orderId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<BusinessException>();


        }

        [Test]
        public async Task Should_cancel_order()
        {

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            var model = new CancelOrderModel { Reason = Guid.NewGuid().ToString() };

            var result = await _orderCommandService.CancelOrderAsync(orderId, model);

            result.IsSuccess.Should().BeTrue();

            Assert.That(await TestHarness.Published.Any<OrderCancelledEvent>());

        }


        [Test]
        public async Task Should_return_error_result_while_cancelling_when_order_is_not_exist()
        {
            

            var model = new CancelOrderModel { Reason = Guid.NewGuid().ToString() };

            var result = await _orderCommandService.CancelOrderAsync(Guid.NewGuid(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();

        }



        [Test]
        public async Task Should_return_error_result_while_cancelling_when_order_is_already_cancelled()
        {
            Guid orderId = Guid.NewGuid();

            await GenerateFakeCancelledOrder(orderId);

            var model = new CancelOrderModel { Reason = Guid.NewGuid().ToString() };

            var result = await _orderCommandService.CancelOrderAsync(orderId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<BusinessException>();

        }

    }

}
