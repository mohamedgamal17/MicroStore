using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using System.Linq.Dynamic.Core;
using System.Net;

namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Authorize]
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
     //   [RequiredScope(BillingScope.Payment.Write)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] PaymentRequestModel model)
        {

            var paymentModel = new CreatePaymentRequestModel
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNumber,
                UserId = CurrentUser.Id.ToString()!,
                TaxCost = model.TaxCost,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubTotal,
                TotalCost = model.TotalCost,
                Items = model.Items,
            };

            

            var result = await _paymentRequestCommandService.CreateAsync(paymentModel);

            return FromResult(result, HttpStatusCode.Created);
        }

        [HttpPost]
        [Route("process/{paymentId}")]
   //     [RequiredScope(BillingScope.Payment.Write)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcessResultDto))]
        public async Task<IActionResult> ProcessPaymentRequest(string paymentId, [FromBody] ProcessPaymentRequestModel model)
        {
            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentId, model);

            return FromResult(result, HttpStatusCode.OK);
        }



        [HttpGet]
        [Route("")]
    //    [RequiredScope(BillingScope.Payment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<PaymentRequestListDto>))]
        public async Task<IActionResult> RetriveUserPaymentRequestList([FromQuery] PagingAndSortingParamsQueryString @params)
        {
            var queryparams = new PagingAndSortingQueryParams
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await _paymentRequestQueryService.ListPaymentAsync(queryparams, CurrentUser.Id.ToString()!);

            return FromResult(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("order_id/{orderId}")]
      //  [RequiredScope(BillingScope.Payment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> RetrivePaymentRequestWithOrderId(string orderId)
        {
            var result = await _paymentRequestQueryService.GetByOrderIdAsync(orderId);

            return FromResult(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("order_number/{orderId}")]
        //  [RequiredScope(BillingScope.Payment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> RetrivePaymentRequestWithOrderNUmber(string orderNumber)
        {
            var result = await _paymentRequestQueryService.GetByOrderNumberAsync(orderNumber);

            return FromResult(result, HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{paymentId}")]
      //  [RequiredScope(BillingScope.Payment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestDto))]
        public async Task<IActionResult> RetrivePaymentRequest(string paymentId)
        {
            var result = await _paymentRequestQueryService.GetAsync(paymentId);

            return FromResult(result, HttpStatusCode.OK);
        }

    }
}
