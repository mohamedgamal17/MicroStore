using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using System.Net;
namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
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
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PagedResult<PaymentRequestListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Envelope))]
        public async Task<IActionResult> RetrivePaymentRequestList([FromQuery] PagingAndSortingParamsQueryString @params, [FromQuery(Name = "user_id")] string? userId = null)
        {
            var queryparams = new PagingAndSortingQueryParams
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await _paymentRequestQueryService.ListPaymentAsync(queryparams, userId);

            return FromResultV2(result, HttpStatusCode.OK);
        }

       


        [HttpGet]
        [Route("order_id/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]

        public async Task<IActionResult> RetrivePaymentRequestWithOrderId(string orderId)
        {
            var result = await _paymentRequestQueryService.GetByOrderIdAsync(orderId);

            return FromResultV2(result, HttpStatusCode.OK); 
        }


        [HttpGet]
        [Route("order_number/{orderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]

        public async Task<IActionResult> RetrivePaymentRequestWithOrderNumber(string orderNumber)
        {
            var result = await _paymentRequestQueryService.GetByOrderNumberAsync(orderNumber);

            return FromResultV2(result, HttpStatusCode.OK); 
        }

        [HttpGet]
        [Route("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> RetrivePaymentRequest(string paymentId)
        {
            var result = await _paymentRequestQueryService.GetAsync(paymentId);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequestModel model)
        {
            var result = await _paymentRequestCommandService.CreateAsync(model);

            return FromResultV2(result, HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("process/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcessResultDto))]
        public async Task<IActionResult> ProcessPaymentRequest(string paymentId, [FromBody] ProcessPaymentRequestModel model)
        {
            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentId,model);

            return FromResultV2(result, HttpStatusCode.OK);
        }
    }
}
