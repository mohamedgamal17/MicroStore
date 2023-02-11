using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.Domain.Security;

namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/user/orders")]
    public class UserOrderController : MicroStoreApiController
    {
        private readonly IApplicationCurrentUser _applicationCurrentUser;

        public UserOrderController(IApplicationCurrentUser applicationCurrentUser)
        {
            _applicationCurrentUser = applicationCurrentUser;
        }

        [HttpGet]
        [Route("")]
        [RequiredScope(InventoryScope.Order.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<OrderListDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetirveUserOrderList([FromQuery] PagingParamsQueryString @params)
        {
            var query = new GetUserOrderListQuery
            {
                UserId = _applicationCurrentUser.Id,
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
