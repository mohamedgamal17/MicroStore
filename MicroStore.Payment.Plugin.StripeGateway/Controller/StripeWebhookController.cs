using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Plugin.StripeGateway.Config;
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

        private readonly ISettingsRepository _settingsRepository;
        public StripeWebhookController(ILogger<StripeWebhookController> logger, IPaymentRequestManager paymentRequestManager, ISettingsRepository settingsRepository)
        {
            _logger = logger;
            _paymentRequestManager = paymentRequestManager;
            _settingsRepository = settingsRepository;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {


            try
            {
                var settings = await _settingsRepository.TryToGetSettings<StripePaymentSettings>(StripePaymentConst.Provider);

                using var stream = new StreamReader(HttpContext.Request.Body);

                var json = await stream.ReadToEndAsync();

                var stripeSignature = HttpContext.Request.Headers["Stripe-Signature"];

                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, settings?.EndPointSecret ?? string.Empty);

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
