using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;

namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/inventory/products")]
    public class OrderController : MicroStoreApiController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<OrderListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveOrderList(PagingQueryParams @params)
        {
            var query = new GetOrderListQuery
            {
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<OrderListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveUserOrderList(string userId,PagingQueryParams @params)
        {
            var query = new GetUserOrderListQuery
            {
                UserId = userId,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpGet]
        [Route("external_order_id/{externalId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveOrderWithExternalId(string externalId)
        {
            var query = new GetOrderWithExternalIdQuery
            {
                ExternalOrderId = externalId
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveOrder(Guid orderId)
        {
            var query = new GetOrderQuery
            {
                OrderId = orderId
            };

            var result = await Send(query);

            return FromResult(result);
        }
    }
}
