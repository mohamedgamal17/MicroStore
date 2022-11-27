//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using MicroStore.BuildingBlocks.Results;
//using MicroStore.Payment.Domain.Shared;
//using MicroStore.Payment.Domain.Shared.Dtos;
//using MicroStore.Payment.Plugin.StripeGateway.Config;
//using MicroStore.Payment.Plugin.StripeGateway.Consts;
//using Stripe;
//using Volo.Abp;

//namespace MicroStore.Payment.Plugin.StripeGateway.Controller
//{
//    [ApiController]
//    [RemoteService(Name = "StripePayment")]
//    [Route("api/[controller]")]
//    public class StripePaymentController : ControllerBase
//    {

//        private readonly IPaymentRequestService _paymentRequestService;

//        private readonly PaymentIntentService _stripePaymentIntentService;

//        private readonly StripePaymentOption _stripePaymentOption;


//        private readonly ILogger<StripePaymentController> _logger;
//        public StripePaymentController(IPaymentRequestService paymentRequestService, PaymentIntentService stripePaymentIntentService, StripePaymentOption stripePaymentOption, ILogger<StripePaymentController> logger)
//        {
//            _paymentRequestService = paymentRequestService;
//            _stripePaymentIntentService = stripePaymentIntentService;
//            _stripePaymentOption = stripePaymentOption;
//            _logger = logger;
//        }

//        [Route("start")]
//        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(PaymentStartedDto))]
//        [HttpPut]
//        public async Task<IActionResult> StartPayment(Guid paymentId)
//        {

//            Result<PaymentDto> paymentrequestResult = await _paymentRequestService.GetPayment(paymentId);

//            if (paymentrequestResult.IsFailure)
//            {
//                return NotFound(paymentrequestResult.Error);
//            }

//            PaymentDto paymentRequest = paymentrequestResult.Value;

//            PaymentIntentCreateOptions paymentIntentCreateOption = new PaymentIntentCreateOptions
//            {
//                Amount = (long)(paymentRequest.Amount * 100),
//                Currency = _stripePaymentOption.Currency,
//                PaymentMethodTypes = _stripePaymentOption.PaymentMethods,

//            };

//            var paymentIntent = await _stripePaymentIntentService.CreateAsync(paymentIntentCreateOption, new RequestOptions { ApiKey = _stripePaymentOption.ApiKey });

//            var result = await _paymentRequestService.StartPayment(paymentId, paymentIntent.Id, StripePaymentDefault.Provider, paymentIntent.Created);


//            if (result.IsFailure)
//            {
//                return BadRequest(result.Error);
//            }

//            return Ok(result.Value);
//        }


//        [Route("webhook")]
//        [HttpPost]
       
//        public async Task<IActionResult> HandleWebHook()
//        {
//            try
//            {
//                using var stream = new StreamReader(HttpContext.Request.Body);

//                var json = await stream.ReadToEndAsync();

//                var stripeSignature = HttpContext.Request.Headers["Stripe-Signature"];

//                var stripeEvent =  EventUtility.ConstructEvent(json, stripeSignature, _stripePaymentOption.EndPointSecret);

//                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

//                await _paymentRequestService.CompletePayment(paymentIntent.Id, StripePaymentDefault.Provider, DateTime.UtcNow);

//                return Ok();

//            }catch(StripeException ex)
//            {
//                return BadRequest();

//            }catch(Exception ex)
//            {
//                _logger.LogException(ex);
          
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//    }
//}
