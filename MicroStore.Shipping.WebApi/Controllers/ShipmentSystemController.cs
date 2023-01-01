using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.WebApi.Models;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/systems")]
    public class ShipmentSystemController : MicroStoreApiController
    {

        [HttpPut]
        [Route("{systemName}")]
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
