using FluentAssertions;
using MicroStore.Payment.Domain.Shared.Domain;
using MicroStore.Payment.IntegrationEvents;
namespace MicroStore.Payment.Application.Tests.Consumers
{
    public class When_consuming_create_payment_request_integration_event : BaseTestFixture
    {

        [Test]
        public async Task Should_create_payment_request_and_publish_payment_created_integation_event()
        {
            CreatePaymentRequestIntegrationEvent createPaymentIntegrationEvent = new CreatePaymentRequestIntegrationEvent
            {
                OrderId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                SubTotal = 50,
                TotalCost = 50,
                Items = new List<IntegrationEvents.Models.PaymentRequestProductModel>
                {
                    new IntegrationEvents.Models.PaymentRequestProductModel
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

            await TestHarness.Bus.Publish(createPaymentIntegrationEvent);

            Assert.That(await TestHarness.Consumed.Any<CreatePaymentRequestIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<CreatePaymentRequestIntegrationEvent>());

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.OrderId == createPaymentIntegrationEvent.OrderId);

            paymentRequest.OrderId.Should().Be(createPaymentIntegrationEvent.OrderId);
            paymentRequest.OrderNumber.Should().Be(createPaymentIntegrationEvent.OrderNumber);
            paymentRequest.TaxCost.Should().Be(createPaymentIntegrationEvent.TaxCost);
            paymentRequest.ShippingCost.Should().Be(createPaymentIntegrationEvent.ShippingCost);
            paymentRequest.TotalCost.Should().Be(createPaymentIntegrationEvent.TotalCost);
            paymentRequest.CustomerId.Should().Be(createPaymentIntegrationEvent.UserId);
            paymentRequest.State.Should().Be(PaymentStatus.Waiting);

        }


    }
}
