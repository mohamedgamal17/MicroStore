using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Api.Models.PaymentRequests;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using MicroStore.Payment.Domain.Security;

namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    [Authorize]
    public class PaymentRequestController : MicroStoreApiController
    {
        [HttpGet]
        [Route("")]
        [RequiredScope(BillingScope.Payment.List)]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<PagedResult<PaymentRequestListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(Envelope))]
        public async Task<IActionResult> RetrivePaymentRequestList([FromQuery] PagingAndSortingQueryParams @params)
        {
            var query = new GetPaymentRequestListQuery
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("user/{userId}")]
        [RequiredScope(BillingScope.Payment.List)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<PaymentRequestListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveUserPaymentRequestList(string userId, [FromQuery] PagingAndSortingQueryParams @params)
        {
            var query = new GetUserPaymentRequestListQuery
            {
                UserId = userId,
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpGet]
        [Route("order_id/{orderId}")]
        [RequiredScope(BillingScope.Payment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PaymentRequestDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetrivePaymentRequestWithOrderId(string orderId)
        {
            var query = new GetPaymentRequestWithOrderIdQuery
            {
                OrderId = orderId
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{paymentId}")]
        [RequiredScope(BillingScope.Payment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PaymentRequestDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetrivePaymentRequest(Guid paymentId)
        {
            var query = new GetPaymentRequestQuery
            {
                PaymentRequestId = paymentId
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpPost]
        [Route("")]
        [RequiredScope(BillingScope.Payment.Create)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PaymentRequestCreatedDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequestModel model)
        {
            var command = new CreatePaymentRequestCommand
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNubmer,
                UserId = model.UserId,
                TaxCost = model.TaxCost,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubtTotal,
                TotalCost = model.TotalCost,
                Items = model.Items,
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("process/{paymentId}")]
        [RequiredScope(BillingScope.Payment.Process)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcessResultDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<PaymentProcessResultDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]

        public async Task<IActionResult> ProcessPaymentRequest(Guid paymentId, [FromBody] ProcessPaymentRequestModel model)
        {
            var command = new ProcessPaymentRequestCommand
            {
                PaymentId = paymentId,
                PaymentGatewayName = model.PaymentGatewayName,
                ReturnUrl = model.ReturnUrl,
                CancelUrl = model.CancelUrl,
            };


            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("complete")]
        [RequiredScope(BillingScope.Payment.Complete)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentRequestCompletedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<PaymentRequestCompletedDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CompletePaymentRequest([FromBody] CompletePaymentRequestModel model)
        {
            var command = new CompletePaymentRequestCommand
            {
                PaymentGatewayName = model.PaymentGatewayName,
                Token = model.Token,
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
