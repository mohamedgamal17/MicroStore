using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Application.Security;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
    public class PaymentRequestController : MicroStoreApiController
    {

        private readonly IPaymentRequestQueryService _paymentRequestQueryService;

        private readonly IPaymentRequestCommandService _paymentRequestCommandService;

        public PaymentRequestController(IPaymentRequestQueryService paymentRequestQueryService, IPaymentRequestCommandService paymentRequestCommandService)
        {
            _paymentRequestQueryService = paymentRequestQueryService;
            _paymentRequestCommandService = paymentRequestCommandService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PagedResult<PaymentRequestDto>))]
        public async Task<IActionResult> RetrivePaymentRequestList([FromQuery] PaymentRequestListQueryModel queryparams, [FromQuery(Name = "user_id")] string? userId = null)
        {
            var result = await _paymentRequestQueryService.ListPaymentAsync(queryparams, userId);

            return result.ToOk();
        }


        [HttpGet]
        [Route("order_id/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]

        public async Task<IActionResult> RetrivePaymentRequestWithOrderId(string orderId)
        {
            var result = await _paymentRequestQueryService.GetByOrderIdAsync(orderId);

            return result.ToOk();
        }


        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]

        public async Task<IActionResult> RetrivePaymentRequestWithOrderNumber(string orderNumber)
        {
            var result = await _paymentRequestQueryService.GetByOrderNumberAsync(orderNumber);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{paymentId}")]
        [ActionName(nameof(RetrivePaymentRequest))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]

        public async Task<IActionResult> RetrivePaymentRequest(string paymentId)
        {
            var result = await _paymentRequestQueryService.GetAsync(paymentId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]

        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequestModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _paymentRequestCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(RetrivePaymentRequest), new { paymentId = result.Value?.Id});
        }

        [HttpPost]
        [Route("process/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcessResultDto))]

        public async Task<IActionResult> ProcessPaymentRequest(string paymentId, [FromBody] ProcessPaymentRequestModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentId,model);

            return result.ToOk();
        }


        [HttpPost]
        [Route("complete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> CompletePaymentRequest(CompletePaymentModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _paymentRequestCommandService.CompleteAsync(model);

            return result.ToOk();
        }


        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(PaymentRequestDto))]

        public async Task<IActionResult> Search([FromBody]PaymentRequestSearchModel model)
        {
            var validationResult = await ValidateModel(model);
            
            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _paymentRequestQueryService.SearchByOrderNumber(model);

            return result.ToOk();
        }
    }
}
