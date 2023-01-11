using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using MicroStore.Shipping.Domain.Security;
using MicroStore.Shipping.WebApi.Models.Systems;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/systems")]
    public class ShipmentSystemController : MicroStoreApiController
    {

        [HttpGet]
        [Route("")]
        [RequiredScope(ShippingScope.System.List)]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(Envelope<ListResultDto<ShipmentSystemDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveShipmentSystemList()
        {
            var query = new GetShipmentSystemListQuery();

            var result = await Send(query);

            return FromResult(result);           
        }

        [HttpGet]
        [Route("system_name/{name}")]
        [RequiredScope(ShippingScope.System.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ListResultDto<ShipmentSystemDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveShipmentSystemWithName(string name)
        {
            var query = new GetShipmentSystemWithNameQuery
            {
                Name = name
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpGet]
        [Route("{systemId}")]
        [RequiredScope(ShippingScope.System.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ListResultDto<ShipmentSystemDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveShipmentSystem(Guid systemId)
        {
            var query = new GetShipmentSystemQuery
            {
                SystemId = systemId
            };

            var result = await Send(query);

            return FromResult(result);
        }

        [HttpPut]
        [Route("{systemName}")]
        [RequiredScope(ShippingScope.System.Update)]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(Envelope))]
        public async Task<IActionResult> UpdateShipmentSystem(string systemName, [FromBody] UpdateShippingSystemModel model)
        {
            var command = new UpdateShippingSystemCommand
            {
                SystemName = systemName,
                IsEnabled = model.IsEnabled,
            };

            var result = await Send(command);

            return FromResult(result);

        }
    }
}
