using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using MicroStore.Shipping.Domain.Security;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/user/shipments")]
    public class UserShipmentController : MicroStoreApiController
    {
        private readonly IApplicationCurrentUser _applicationCurrentUser;

        public UserShipmentController(IApplicationCurrentUser applicationCurrentUser)
        {
            _applicationCurrentUser = applicationCurrentUser;
        }


        [HttpGet]
        [Route("")]
        [RequiredScope(ShippingScope.Shipment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<ShipmentDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveUserShipmentList( [FromQuery] PagingParamsQueryString @params)
        {
            var query = new GetUserShipmentListQuery
            {
                UserId = _applicationCurrentUser.Id,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("order_id/{orderId}")]
        [RequiredScope(ShippingScope.Shipment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<ShipmentDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveShipmentWithOrderId(string orderId)
        {
            var query = new GetShipmentWithOrderIdQuery
            {
                OrderId = orderId
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{shipmentId}")]
        [RequiredScope(ShippingScope.Shipment.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<ShipmentDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveShipment(Guid shipmentId)
        {
            var query = new GetShipmentQuery
            {
                ShipmentId = shipmentId
            };

            var result = await Send(query);

            return FromResult(result);
        }
    }
}
