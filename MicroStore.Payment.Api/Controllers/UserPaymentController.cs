using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Payment.Api.Models.PaymentRequests;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Security;
using System.Linq.Dynamic.Core;

namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/user/payments")]
    public class UserPaymentController : MicroStoreApiController
    {

        private readonly IApplicationCurrentUser _applicationCurrentUser;

        public UserPaymentController(IApplicationCurrentUser applicationCurrentUser)
        {
            _applicationCurrentUser = applicationCurrentUser;
        }

        [HttpPost]
        [Route("")]
        [RequiredScope(BillingScope.Payment.Create)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PaymentRequestDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] PaymentRequestModel model)
        {
            var command = new CreatePaymentRequestCommand
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNubmer,
                UserId = _applicationCurrentUser.Id,
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PaymentRequestDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<PaymentRequestDto>))]
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


        [HttpGet]
        [Route("")]
        [RequiredScope(BillingScope.Payment.List)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<PaymentRequestListDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveUserPaymentRequestList([FromQuery] PagingAndSortingQueryParams @params)
        {
            var query = new GetUserPaymentRequestListQuery
            {
                UserId = _applicationCurrentUser.Id,
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

    }
}
