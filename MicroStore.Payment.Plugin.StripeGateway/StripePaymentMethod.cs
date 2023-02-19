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
using System.Net;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
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

        public StripePaymentMethod(SessionService sessionService, PaymentIntentService paymentIntentService, IPaymentRequestManager paymentRequestRepository, IObjectMapper objectMapper,  ISettingsRepository settingsRepository, RefundService refundService, ILogger<StripePaymentMethod> logger)
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

        public async Task<ResponseResult<PaymentRequestDto>> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult<PaymentRequestDto>(async () =>
            {
                var requestOptions = await PrepareStripeRequest(cancellationToken);

                var session = await _sessionService
                    .GetAsync(completePaymentModel.Token, new SessionGetOptions(), requestOptions, cancellationToken);

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Stripe payment session {session}", session);
                }

                if (session.Status != "complete")
                {
                    var error = new ErrorInfo
                    {
                        Message = $"checkout session with id : {session.Id} is not completed yet"
                    };

                    return Failure<PaymentRequestDto>(HttpStatusCode.BadRequest, error);

                }

               var result =  await _paymentRequestManager.Complete(Guid.Parse(session.ClientReferenceId), PaymentGatewayName, session.PaymentIntentId, DateTime.UtcNow, cancellationToken);

                return Success(HttpStatusCode.OK, result);
            });

        }

        public async Task<ResponseResult<PaymentProcessResultDto>> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult<PaymentProcessResultDto>(HttpStatusCode.OK, async () =>
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
                    SuccessUrl = session.SuccessUrl + $"?token{session.Id}" /*+ "&" + $"gate={StripePaymentConst.Provider}"*/,
                    CancelUrl = session.CancelUrl + $"?token{session.Id}" /*+ "&" + $"gate={StripePaymentConst.Provider}"*/,
                    CheckoutLink = session.Url
                };
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

        public async Task<ResponseResult<PaymentRequestDto>> Refund(Guid paymentId, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult<PaymentRequestDto>(HttpStatusCode.OK, async () =>
            {
                var  paymentRequest = await _paymentRequestManager.GetPaymentRequest(paymentId);

                var requestOptions = await PrepareStripeRequest();

                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = paymentRequest.TransctionId
                };

                var refundObject = await _refundService.CreateAsync(refundOptions, requestOptions, cancellationToken);

                return await _paymentRequestManager.Refund(paymentId, DateTime.UtcNow, string.Empty, cancellationToken);
            });
        }



        private async Task<ResponseResult<T>> WrappResponseResult<T>(HttpStatusCode httpStatusCode, Func<Task<T>> func)
        {

            return await WrappResponseResult(async () =>
            {
                var result = await func();

                return Success(httpStatusCode, result);
            });
        }

        private async Task<ResponseResult<T>> WrappResponseResult<T>(Func<Task<ResponseResult<T>>> func)
        {
            try
            {
                return await func();

            }
            catch (StripeException ex)
            {
                return Failure<T>(HttpStatusCode.BadRequest, ConvertStripeError(ex.StripeError));

            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                return Failure<T>(HttpStatusCode.BadRequest, new ErrorInfo
                {
                    Message = "Stripe gateway is not available now"
                });
            }
        }


        private ResponseResult<T> Success<T>(HttpStatusCode statusCode, T result)
            => ResponseResult.Success<T>((int)statusCode, result);

        private ResponseResult Success(HttpStatusCode statusCode)
            => ResponseResult.Success((int)statusCode);

        private ResponseResult<T> Failure<T>(HttpStatusCode statusCode, ErrorInfo error)
            => ResponseResult.Failure<T>((int)statusCode, error);

        private ResponseResult Failure(HttpStatusCode statusCode, ErrorInfo error)
        => ResponseResult.Failure((int)statusCode, error);

        private ErrorInfo ConvertStripeError(StripeError stripeError)
        {
            return new ErrorInfo
            {
                Source = stripeError.Source?.Object,
                Type = stripeError.Type,
                Message = stripeError.Error,
                Details = stripeError.Message
            };
        }




    }
}