﻿using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Abstractions.Common;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Plugin.StripeGateway.Config;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Stripe;
using Stripe.Checkout;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
namespace MicroStore.Payment.Plugin.StripeGateway
{
    [ExposeServices(typeof(IPaymentMethod),IncludeDefaults = true, IncludeSelf = true)]
    public class StripePaymentMethod : IPaymentMethod , IUnitOfWorkEnabled , ITransientDependency
    {

        private readonly SessionService _sessionService;

        private readonly PaymentIntentService _paymentIntentService;

        private readonly RefundService _refundService;

        private readonly IPaymentRequestRepository _paymentRequestRepository;

        private readonly ISettingsRepository _settingsRepository;

        private readonly IObjectMapper _objectMapper;

        private readonly IRepository<PaymentSystem> _paymentSystemRepository;

        public StripePaymentMethod(SessionService sessionService, PaymentIntentService paymentIntentService, IPaymentRequestRepository paymentRequestRepository, IObjectMapper objectMapper, IRepository<PaymentSystem> paymentSystemRepository, ISettingsRepository settingsRepository, RefundService refundService)
        {
            _sessionService = sessionService;
            _paymentIntentService = paymentIntentService;
            _paymentRequestRepository = paymentRequestRepository;
            _objectMapper = objectMapper;
            _paymentSystemRepository = paymentSystemRepository;
            _settingsRepository = settingsRepository;
            _refundService = refundService;
        }
        public string PaymentGatewayName => StripePaymentConst.Provider;

        public async Task<PaymentRequestCompletedDto> Complete(CompletePaymentModel completePaymentModel, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestOptions = await PrepareStripeRequest(cancellationToken);

                var session = await _sessionService
                    .GetAsync(completePaymentModel.Token, new SessionGetOptions(), requestOptions, cancellationToken);

                if(session.Status != "unpaid")
                {
                    throw new BusinessException("your payment session is uncompleted");
                }

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
            var paymentRequest = await _paymentRequestRepository.GetPaymentRequest(paymentId);

            

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

            var  session = await _sessionService.CreateAsync(options, requestOptions, cancellationToken);

            return new PaymentProcessResultDto
            {
                SessionId = session.Id,
                TransactionId=  session.PaymentIntentId,
                AmountSubTotal=  (session.AmountSubtotal / 100 ?? 0 ),
                AmountTotal=  (session.AmountTotal / 100  ?? 0 ),
                SuccessUrl = session.SuccessUrl+ $"?token{session.Id}" + $"?gate={StripePaymentConst.Provider}",
                CancelUrl = session.CancelUrl + $"?token{session.Id}" + $"?gate={StripePaymentConst.Provider}",              
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
                        Images = new List<string> { x.Thumbnail ?? string.Empty},
                        
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

        public async Task Refund(Guid paymentId, CancellationToken cancellationToken = default)
        {

            PaymentRequest paymentRequest = (await _paymentRequestRepository
                .GetPaymentRequest(paymentId))!;
     
            var requestOptions = await PrepareStripeRequest();

            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = paymentRequest.TransctionId
            };

            var refundObject =  await _refundService.CreateAsync(refundOptions, requestOptions, cancellationToken);
 
            paymentRequest.MarkAsRefunded(DateTime.UtcNow, string.Empty);

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

        }


        public async Task<bool> IsEnabled()
        {
            var paymentSystem = await _paymentSystemRepository.SingleAsync(x => x.Name == StripePaymentConst.Provider);

            return paymentSystem.IsEnabled;
        }


    }
}
