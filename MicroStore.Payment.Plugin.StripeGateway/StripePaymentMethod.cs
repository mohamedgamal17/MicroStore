using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using MicroStore.Payment.Plugin.StripeGateway.Config;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Stripe;
using Stripe.Checkout;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
namespace MicroStore.Payment.Plugin.StripeGateway
{
    [ExposeServices(typeof(IPaymentMethod), IncludeDefaults = true, IncludeSelf = true)]
    public class StripePaymentMethod : IPaymentMethod, IUnitOfWorkEnabled, ITransientDependency
    {

        private readonly SessionService _sessionService;

        private readonly PaymentIntentService _paymentIntentService;

        private readonly RefundService _refundService;

        private readonly IPaymentRequestManager _paymentRequestManager;

        private readonly ISettingsRepository _settingsRepository;

        private readonly IObjectMapper _objectMapper;


        private readonly ILogger<StripePaymentMethod> _logger;

        public StripePaymentMethod(SessionService sessionService, PaymentIntentService paymentIntentService, IPaymentRequestManager paymentRequestRepository, IObjectMapper objectMapper, ISettingsRepository settingsRepository, RefundService refundService, ILogger<StripePaymentMethod> logger)
        {
            _sessionService = sessionService;
            _paymentIntentService = paymentIntentService;
            _paymentRequestManager = paymentRequestRepository;
            _objectMapper = objectMapper;
            _settingsRepository = settingsRepository;
            _refundService = refundService;
            _logger = logger;
        }
        public string PaymentGatewayName => StripePaymentConst.Provider;



        public async Task<ResultV2<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult(async () =>
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


                var requestOptions = await PrepareStripeRequest(cancellationToken);

                var session = await _sessionService.CreateAsync(options, requestOptions, cancellationToken);

                return new PaymentProcessResultDto
                {
                    SessionId = session.Id,
                    TransactionId = session.PaymentIntentId,
                    AmountSubTotal = (session.AmountSubtotal / 100 ?? 0),
                    AmountTotal = (session.AmountTotal / 100 ?? 0),
                    SuccessUrl = session.SuccessUrl,
                    CancelUrl = session.CancelUrl,
                    CheckoutLink = session.Url
                };
            });

        }

        public async Task<ResultV2<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult(async () =>
            {
                var paymentRequest = await _paymentRequestManager.GetPaymentRequest(paymentId);

                var requestOptions = await PrepareStripeRequest();

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

        private async Task<RequestOptions> PrepareStripeRequest(CancellationToken cancellationToken = default)
        {
            var settings = await _settingsRepository.TryToGetSettings<StripePaymentSettings>(StripePaymentConst.Provider, cancellationToken);

            return new RequestOptions
            {
                ApiKey = settings?.ApiKey ?? StripeConfiguration.ApiKey
            };
        }





        private async Task<ResultV2<T>> WrappResponseResult<T>(Func<Task<T>> func)
        {

            try
            {
                return await func();

            }
            catch (StripeException ex)
            {
                return new ResultV2<T>(new BusinessException(message: ex.StripeError?.Message, details: ex.StripeError?.ErrorDescription));

            }
        }


        private ErrorInfo ConvertStripeError(StripeError stripeError)
        {
            return ErrorInfo.BusinessLogic(stripeError.Message);

        }




    }
}