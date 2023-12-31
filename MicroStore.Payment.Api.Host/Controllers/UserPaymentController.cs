using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Application.Security;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using System.Linq.Dynamic.Core;
namespace MicroStore.Payment.Api.Host.Controllers
{
    [ApiController]
    [Route("api/user/payments")]
    public class UserPaymentController : MicroStoreApiController
    {
        private readonly IPaymentRequestCommandService _paymentRequestCommandService;

        private readonly IPaymentRequestQueryService _paymentRequestQueryService;

        private readonly ILogger<UserPaymentController> _logger;
        public UserPaymentController(IPaymentRequestCommandService paymentRequestCommandService, IPaymentRequestQueryService paymentRequestQueryService, ILogger<UserPaymentController> logger)
        {
            _paymentRequestCommandService = paymentRequestCommandService;
            _paymentRequestQueryService = paymentRequestQueryService;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] PaymentRequestModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var paymentModel = new CreatePaymentRequestModel
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNumber,
                UserId = CurrentUser.UserId!,
                TaxCost = model.TaxCost,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubTotal,
                TotalCost = model.TotalCost,
                Items = model.Items,
            };



            var result = await _paymentRequestCommandService.CreateAsync(paymentModel);

            return result.ToCreatedAtAction(nameof(RetrivePaymentRequest), new { paymentId = result.Value?.Id });
        }

        [HttpPost]
        [Route("process/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcessResultDto))]
        [Authorize(Policy = ApplicationPolicies.RequirePaymentReadScope)]

        public async Task<IActionResult> ProcessPaymentRequest(string paymentId, [FromBody] ProcessPaymentRequestModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentId, model);

            return result.ToOk();
        }

        [HttpPost]
        [Route("complete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
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


        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<PaymentRequestDto>))]
        [Authorize(Policy = ApplicationPolicies.RequirePaymentReadScope)]
        public async Task<IActionResult> RetriveUserPaymentRequestList([FromQuery] PaymentRequestListQueryModel queryparams)
        {
            var result = await _paymentRequestQueryService.ListPaymentAsync(queryparams, CurrentUser.UserId!);

            return result.ToOk();
        }

        [HttpGet]
        [Route("order_id/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        [Authorize(Policy = ApplicationPolicies.RequirePaymentReadScope)]
        public async Task<IActionResult> RetrivePaymentRequestWithOrderId(string orderId)
        {
            var result = await _paymentRequestQueryService.GetByOrderIdAsync(orderId);

            return result.ToOk();
        }

        [HttpGet]
        [Route("order_number/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        [Authorize(Policy = ApplicationPolicies.RequirePaymentReadScope)]
        public async Task<IActionResult> RetrivePaymentRequestWithOrderNumber(string orderNumber)
        {
            var result = await _paymentRequestQueryService.GetByOrderNumberAsync(orderNumber);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{paymentId}")]
        [ActionName(nameof(RetrivePaymentRequest))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        [Authorize(Policy = ApplicationPolicies.RequirePaymentReadScope)]
        public async Task<IActionResult> RetrivePaymentRequest(string paymentId)
        {
            var result = await _paymentRequestQueryService.GetAsync(paymentId);

            return result.ToOk();
        }

    }
}
