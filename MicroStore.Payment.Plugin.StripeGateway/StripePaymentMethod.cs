using Microsoft.Extensions.Options;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using MicroStore.Payment.Plugin.StripeGateway.Config;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Stripe;
using Stripe.Checkout;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace MicroStore.Payment.Plugin.StripeGateway
{
    public class StripePaymentMethod : IPaymentMethod , IUnitOfWorkEnabled
    {

        private readonly SessionService _sessionService;

        private readonly PaymentIntentService _paymentIntentService;

        private readonly IPaymentRequestRepository _paymentRequestRepository;

        private readonly StripePaymentOption _options;

        private readonly IObjectMapper _objectMapper;

        public StripePaymentMethod(SessionService sessionService, PaymentIntentService paymentIntentService , IPaymentRequestRepository paymentRequestRepository, IOptions<StripePaymentOption> options, IObjectMapper objectMapper)
        {
            _sessionService = sessionService;
            _paymentIntentService = paymentIntentService;
            _paymentRequestRepository = paymentRequestRepository;
            _options = options.Value;
            _objectMapper = objectMapper;
        }
        public string PaymentGatewayName => StripePaymentDefault.Provider;

        public async Task<PaymentRequestCompletedDto> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default)
        {
            try
            {
                var session = await _sessionService
                    .GetAsync(completePaymentModel.Token, new SessionGetOptions(), PrepareStripeRequest(), cancellationToken);

                PaymentRequest paymentRequest = await _paymentRequestRepository.SingleAsync( x=> x.Id == Guid.Parse(session.ClientReferenceId));

                paymentRequest.Complete(PaymentGatewayName, session.PaymentIntentId, DateTime.UtcNow);

                await  _paymentRequestRepository.UpdateAsync(paymentRequest);

                return _objectMapper.Map<PaymentRequest, PaymentRequestCompletedDto>(paymentRequest);

            }
            catch (StripeException ex)
            {
                throw ex;
            }
        }

        public async Task<PaymentProcessResultDto> Process(Guid paymentId, ProcessPaymentModel processPaymentModel, CancellationToken cancellationToken = default)
        {
            PaymentRequest? paymentRequest = await _paymentRequestRepository.GetPaymentRequest(paymentId);

            if(paymentRequest == null)
            {
                throw new EntityNotFoundException(typeof(PaymentRequest), paymentId);
            }

            var options = new SessionCreateOptions
            {
                SuccessUrl = processPaymentModel.ReturnUrl,
                CancelUrl = processPaymentModel.CancelUrl,
                ClientReferenceId = paymentId.ToString(),
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

                        }

                    },
                },

                Mode = "payment"
            };

            var  session = await _sessionService.CreateAsync(options, PrepareStripeRequest(), cancellationToken);

            return new PaymentProcessResultDto
            {
                CheckoutLink = session.Url
            };
        }


        private List<SessionLineItemOptions> PrepareStripeLineItems(List<PaymentRequestProduct> items)
        {
            return items.Select(x => new SessionLineItemOptions
            {
                Quantity = x.Quantity,

                PriceData = new SessionLineItemPriceDataOptions
                {
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = x.Name,
                        Images = new List<string> { x.Image ?? string.Empty},
                        
                    },

                    UnitAmountDecimal = x.UnitPrice
                }
               
            }).ToList();
        }

        private RequestOptions PrepareStripeRequest()
        {
            return new RequestOptions
            {
                ApiKey = _options.ApiKey
            };
        }

        public async Task Refund(Guid paymentId, CancellationToken cancellationToken = default)
        {

            PaymentRequest paymentRequest = await _paymentRequestRepository
                .SingleAsync(x => x.Id == paymentId);

 
            await  _paymentIntentService.
                CancelAsync(paymentRequest.TransctionId, new PaymentIntentCancelOptions(), PrepareStripeRequest(), cancellationToken);

            paymentRequest.MarkAsRefunded(DateTime.UtcNow, string.Empty);

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

        }


    }
}
