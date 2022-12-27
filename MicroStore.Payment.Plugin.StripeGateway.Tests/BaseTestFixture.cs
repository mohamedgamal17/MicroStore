using MicroStore.Payment.Domain;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using MicroStore.Payment.PluginInMemoryTest;
using Stripe;
using Stripe.Checkout;

namespace MicroStore.Payment.Plugin.StripeGateway.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : PluginTestFixture<StripGatewayTestModule>
    {
        protected SessionService StripeSessionService { get; }
        protected PaymentIntentService StripePaymentIntentService { get; }

        public BaseTestFixture()
        {
            StripeSessionService = GetRequiredService<SessionService>();

            StripePaymentIntentService = GetRequiredService<PaymentIntentService>();
        }


        public Task<PaymentRequest> GenerateWaitingPaymentRequest()
        {

            PaymentRequest paymentRequest = new PaymentRequest()
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                CustomerId =Guid.NewGuid().ToString(),
                ShippingCost = 40,
                SubTotal = 50,
                TaxCost = 10,
                TotalCost = 100,
                Items = new List<PaymentRequestProduct>
                {
                    new PaymentRequestProduct
                    {
                        Sku = Guid.NewGuid().ToString(),
                        Name = Guid.NewGuid().ToString(),
                        ProductId= Guid.NewGuid().ToString(),
                        Quantity = 1,
                        Thumbnail = Guid.NewGuid().ToString(),
                        UnitPrice = 50 
                    }
                }
            };

            return InsertAsync(paymentRequest);

        }
     
        public async Task<Session> GenerateUnPayedStripePaymentSessionRequest(PaymentRequest paymentRequest)
        {
            var options = new SessionCreateOptions
            {
                Customer = "cus_MiXkMEiPQ95svr",
                


                SuccessUrl = "https://cancel.com/",
                CancelUrl = "https://www.success.com/",
                ClientReferenceId = paymentRequest.Id.ToString(),
                LineItems = PrepareStripeLineItems(paymentRequest.Items),
                ShippingOptions = new List<SessionShippingOptionOptions>
                {
                    new SessionShippingOptionOptions
                    {
                        ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                        {
                            FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                            {
                               Amount = Convert.ToInt64(paymentRequest.ShippingCost * 100),
                               Currency = "usd"
                            },

                            DisplayName = "Shipping Cost",
                            Type ="fixed_amount"
                        }

                    },

                    
                },
                SubmitType ="pay",
                
                Mode = "payment"
            };

            var session = await StripeSessionService.CreateAsync(options);

            return session;
        }
     
        public async Task<PaymentIntent> GenerateConfirmedPaymentIntent(PaymentRequest paymentRequest)
        {
            var paymentIntentRequest = await StripePaymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(paymentRequest.TotalCost * 100),
                Currency ="usd",
                PaymentMethodTypes = new List<string> { "card"},
                PaymentMethod= "card_1MJ4dxCNMS50bRuoFH82oZLQ",
                Customer = "cus_MiXkMEiPQ95svr",
                
            });


            return  await StripePaymentIntentService.ConfirmAsync(paymentIntentRequest.Id);
        }

        public async Task<PaymentRequest> GeneratePayedPaymentRequest()
        {
            var paymentRequest = await GenerateWaitingPaymentRequest();

            var paymentIntent = await GenerateConfirmedPaymentIntent(paymentRequest);

            paymentRequest.Complete(StripePaymentConst.Provider, paymentIntent.Id, DateTime.UtcNow);

            return await UpdateAsync(paymentRequest);

        }
        public List<SessionLineItemOptions> PrepareStripeLineItems(List<PaymentRequestProduct> items)
        {
            return items.Select(x => new SessionLineItemOptions
            {
                Quantity = x.Quantity,

                PriceData = new SessionLineItemPriceDataOptions
                {
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = x.Name,
                        Images = new List<string> { x.Thumbnail ?? string.Empty },

                    },

                    UnitAmountDecimal = Convert.ToDecimal(x.UnitPrice * 100),

                    Currency=  "usd"
                }

            }).ToList();
        }


    }
}
