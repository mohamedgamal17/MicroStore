using Microsoft.Extensions.Options;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Configuration;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Stripe;
using Stripe.Checkout;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
namespace MicroStore.Payment.Plugin.StripeGateway
{
 
    [ExposeServices(typeof(IPaymentMethodProvider), IncludeDefaults = true, IncludeSelf = true)]

    public class StripePaymentMethodProvider : IPaymentMethodProvider , IUnitOfWorkEnabled, ITransientDependency
    {

        private readonly PaymentSystem _paymentSystem;

        private readonly SessionService _sessionService;

        private readonly PaymentIntentService _paymentIntentService;

        private readonly RefundService _refundService;

        private readonly IPaymentRequestManager _paymentRequestManager;

        public StripePaymentMethodProvider(IOptions<PaymentSystemOptions> options, SessionService sessionService, PaymentIntentService paymentIntentService, RefundService refundService, IPaymentRequestManager paymentRequestManager)
        {
            _paymentSystem = options.Value.Systems.Single(x=> x.Name == StripePaymentConst.Provider);
            _sessionService = sessionService;
            _paymentIntentService = paymentIntentService;
            _refundService = refundService;
            _paymentRequestManager = paymentRequestManager;
        }

        public async Task<Result<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            return await WrappResponseWithResult(async () =>
            {
                var paymentRequest = await _paymentRequestManager.GetPaymentRequest(paymentId);

                var options = new SessionCreateOptions
                {
                    SuccessUrl = processPaymentModel.ReturnUrl,
                    CancelUrl = processPaymentModel.CancelUrl,
                    ClientReferenceId = paymentId.ToString(),
                    LineItems = PrepareStripeLineItems(paymentRequest!.Items),
                    ShippingOptions = new List<SessionShippingOptionOptions>
                    {
                        new SessionShippingOptionOptions
                        {
                            ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                            {
                                FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                                {
                                   Amount = Convert.ToInt64((paymentRequest.ShippingCost + paymentRequest.TaxCost)  * 100),
                                   Currency = "usd"
                                },


                                DisplayName = "Shipping Cost",

                                Type = "fixed_amount"
                            }

                        },


                    },

                    Metadata = new Dictionary<string, string>
                    {
                        {  "paymentrequest_id" ,  paymentRequest.Id}
                    },



                    Mode = "payment"
                };


                var requestOptions =  PrepareStripeRequest();

                var session = await _sessionService.CreateAsync(options, requestOptions, cancellationToken);

                return new PaymentProcessResultDto
                {
                    SessionId = session.Id,
                    TransactionId = session.PaymentIntentId,
                    AmountSubTotal = (session.AmountSubtotal / 100 ?? 0),
                    AmountTotal = (session.AmountTotal / 100 ?? 0),
                    SuccessUrl = session.SuccessUrl,
                    CancelUrl = session.CancelUrl,
                    CheckoutLink = session.Url,
                    Provider = StripePaymentConst.Provider
                };
            });
        }
        public async Task<Result<PaymentRequestDto>> Complete(string sessionId, CancellationToken cancellationToken = default)
        {
            return await WrappReponseWithException(async () =>
            {
                var requestOptions = PrepareStripeRequest();

                var stripeSession = await _sessionService.GetAsync(sessionId, requestOptions: requestOptions);

                if (stripeSession.Status != "complete")
                {
                    return new Result<PaymentRequestDto>(new UserFriendlyException("Payment session is not completed"));
                }

                var paymentRequestId = stripeSession.Metadata["paymentrequest_id"];

                return await _paymentRequestManager.Complete(paymentRequestId, StripePaymentConst.Provider, stripeSession.PaymentIntentId, DateTime.UtcNow);
            });
        }

        public async Task<Result<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default)
        {
            return await WrappResponseWithResult(async () =>
            {
                var paymentRequest = await _paymentRequestManager.GetPaymentRequest(paymentId);

                var requestOptions =  PrepareStripeRequest();

                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = paymentRequest.TransctionId
                };

                var refundObject = await _refundService.CreateAsync(refundOptions, requestOptions, cancellationToken);

                return await _paymentRequestManager.Refund(paymentId, DateTime.UtcNow, string.Empty, cancellationToken);
            });
        }



        private List<SessionLineItemOptions> PrepareStripeLineItems(List<PaymentRequestProductDto> items)
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
                    Currency = "usd"
                }

            }).ToList();
        }

        private RequestOptions PrepareStripeRequest()
        {
            var settings = _paymentSystem.Configuration;

            return new RequestOptions
            {
                ApiKey = settings.ApiKey ?? StripeConfiguration.ApiKey
            };
        }


        private async Task<T> WrappReponseWithException<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();

            }catch(StripeException ex)
            {
                throw new UserFriendlyException(message: ex.StripeError?.Error, details: ex.StripeError?.ErrorDescription);
            }
        }
        private async Task<Result<T>> WrappResponseWithResult<T>(Func<Task<T>> func)
        {

            try
            {
                return await func();

            }
            catch (StripeException ex)
            {
                return new Result<T>(new UserFriendlyException(message: ex.StripeError?.Message, details: ex.StripeError?.ErrorDescription));
            }
        }
    }
}