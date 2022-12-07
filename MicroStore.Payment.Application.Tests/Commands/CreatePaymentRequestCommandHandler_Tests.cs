using FluentAssertions;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Domain.Shared.Domain;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Payment.Application.Tests.Commands
{
    public class CreatePaymentRequestCommandHandler_Tests : BaseTestFixture
    {

        public async Task Should_create_payment_request()
        {
            CreatePaymentRequestCommand command = new CreatePaymentRequestCommand
            {
                OrderId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                SubTotal = 50,
                TotalCost = 50,
                Items = new List<OrderItemModel>
                {
                    new OrderItemModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        ProductId = Guid.NewGuid().ToString(),
                        Sku = Guid.NewGuid().ToString(),
                        Image = Guid.NewGuid().ToString(),
                        Quantity = 5,
                        UnitPrice = 50
                    }
                }
            };

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.OrderId == command.OrderId);

            paymentRequest.OrderId.Should().Be(command.OrderId);
            paymentRequest.OrderNumber.Should().Be(command.OrderNumber);
            paymentRequest.TaxCost.Should().Be(command.TaxCost);
            paymentRequest.ShippingCost.Should().Be(command.ShippingCost);
            paymentRequest.TotalCost.Should().Be(command.TotalCost);
            paymentRequest.CustomerId.Should().Be(command.UserId);
            paymentRequest.State.Should().Be(PaymentStatus.Waiting);
        }
    }
}
