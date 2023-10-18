using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Configuration;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Stripe;

namespace MicroStore.Payment.Plugin.StripeGateway.Controller
{
    [ApiController]
    [Route("api/plugins/stripewebhook")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly ILogger<StripeWebhookController> _logger;

        private readonly IPaymentRequestManager _paymentRequestManager;


        private readonly PaymentSystem _paymentSystem;

        public StripeWebhookController(ILogger<StripeWebhookController> logger, IPaymentRequestManager paymentRequestManager, IOptions<PaymentSystemOptions> options )
        {
            _logger = logger;
            _paymentRequestManager = paymentRequestManager;
            _paymentSystem = options.Value.Systems.Single(x=> x.Name == StripePaymentConst.Provider);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {

            if (!_paymentSystem.IsEnabled)
            {
                return BadRequest("Stripe payment provider has not been configured");
            }

            try
            {
                var configuration = _paymentSystem.Configuration;

                using var stream = new StreamReader(HttpContext.Request.Body);

                var json = await stream.ReadToEndAsync();

                var stripeSignature = HttpContext.Request.Headers["Stripe-Signature"];

                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, configuration?.WebHookSecret ?? string.Empty);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;


                    string? paymentRequestId;

                    if(checkoutSession!.Metadata.TryGetValue("paymentrequest_id",out paymentRequestId))
                    {
                        await _paymentRequestManager.Complete(paymentRequestId, StripePaymentConst.Provider, checkoutSession.PaymentIntentId, DateTime.UtcNow);
                    }

                }
                else
                {

                    _logger.LogDebug("Unhandled event type: {@EventType}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException ex)
            {
                _logger.LogException(ex, LogLevel.Warning);
                return BadRequest();
            }
        }
    }
}
