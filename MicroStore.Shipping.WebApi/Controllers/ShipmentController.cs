using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using MicroStore.Shipping.Domain.Security;
using MicroStore.Shipping.WebApi.Models.Shipments;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/shipments")]
    public class ShipmentController : MicroStoreApiController
    {

        [HttpGet]
        [Route("")]
        [RequiredScope(ShippingScope.Shipment.List)]
        [ProducesResponseType(StatusCodes.Status200OK,Type= typeof(Envelope<PagedResult<ShipmentDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type= typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type= typeof(Envelope))]
        public async Task<IActionResult> RetriveShipmentList([FromQuery]PagingQueryParams @params)
        {
            var query = new GetShipmentListQuery
            {
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("user/{userId}")]
        [RequiredScope(ShippingScope.Shipment.List)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<ShipmentDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveUserShipmentList(string userId, [FromQuery]  PagingQueryParams @params)
        {
            var query = new GetUserShipmentListQuery
            {
                UserId = userId,
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

        [HttpPost]
        [Route("")]
        [RequiredScope(ShippingScope.Shipment.Create)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ShipmentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentModel createShipmentModel)
        {
            var command = new CreateShipmentCommand
            {
                UserId = createShipmentModel.UserId,
                OrderId = createShipmentModel.OrderId,
                Items = createShipmentModel.Items,
                Address = createShipmentModel.Address
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("fullfill/{shipmentId}")]
        [RequiredScope(ShippingScope.Shipment.Fullfill)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ShipmentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> FullfillShipment(Guid shipmentId, [FromBody] FullfillShipmentModel model)
        {
            var command = new FullfillShipmentCommand
            {
                ShipmentId = shipmentId,
                SystemName = model.SystemName,
                AddressFrom = model.AddressFrom,
                Pacakge = model.Pacakge
            };

            var result = await Send(command);

            return FromResult(result);
        }


     
    }
}
