using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using MicroStore.Inventory.Domain.Security;

namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/inventory/orders")]
    [Authorize]
    public class OrderController : MicroStoreApiController
    {

        [HttpGet]
        [Route("")]
        [RequiredScope(InventoryScope.Order.List)]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<OrderListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveOrderList([FromQuery] PagingQueryParams @params)
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
        [RequiredScope(InventoryScope.Order.List)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<OrderListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveUserOrderList(string userId, [FromQuery]  PagingQueryParams @params)
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
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
