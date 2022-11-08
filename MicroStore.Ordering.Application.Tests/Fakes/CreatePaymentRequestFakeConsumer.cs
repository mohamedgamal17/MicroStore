using MassTransit;
using MicroStore.Payment.IntegrationEvents;
using MicroStore.Payment.IntegrationEvents.Responses;

namespace MicroStore.Ordering.Application.Tests.Fakes
{
    public class CreatePaymentRequestFakeConsumer : IConsumer<CreatePaymentRequest>
    {
        public Task Consume(ConsumeContext<CreatePaymentRequest> context)
        {
            return context.RespondAsync(new PaymentCreatedResponse
            {
                TransactionId = "FakeTransactionId",
                Gateway = "FakeGateway"
            });
        }
    }
}
