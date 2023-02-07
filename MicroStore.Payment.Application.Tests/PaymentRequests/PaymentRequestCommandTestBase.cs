using MicroStore.Payment.Domain;

namespace MicroStore.Payment.Application.Tests.PaymentRequests
{
    public class PaymentRequestCommandTestBase : BaseTestFixture
    {    
        public Task<PaymentRequest> CreateFakePaymentRequest()
        {
            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                TotalCost = 50,
            };

            return Insert(paymentRequest);
        }
        public async Task<PaymentRequest> CreateFakeCompletedPaymentRequest()
        {
            var fakePaymentRequest = await CreateFakePaymentRequest();

            fakePaymentRequest.Complete(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.UtcNow);

            return await Update(fakePaymentRequest);
        }
    }
}
