using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain.Shared.Models;

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
                UserId = Guid.NewGuid().ToString(),
                TotalCost = 50,
            };

            return Insert(paymentRequest);
        }
        public async Task<PaymentRequest> CreateFakeCompletedPaymentRequest()
        {
            var fakePaymentRequest = await CreateFakePaymentRequest();

            fakePaymentRequest.Complete(PaymentMethodConst.PaymentGatewayName, Guid.NewGuid().ToString(), DateTime.UtcNow);

            return await Update(fakePaymentRequest);
        }

        public CreatePaymentRequestModel GeneratePaymentRequestModel()
        {
            return new CreatePaymentRequestModel
            {
                OrderId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                SubTotal = 50,
                TotalCost = 50,
                Items = new List<PaymentProductModel>
                {
                    new PaymentProductModel
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
        }
    }
}
