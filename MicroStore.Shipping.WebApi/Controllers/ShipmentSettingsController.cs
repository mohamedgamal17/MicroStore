using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.WebApi.Models;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class ShipmentSettingsController : MicroStoreApiController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(Envelope))]
        public Task<IActionResult> GetShipmentSettings()
        {
            throw new NotImplementedException();
        }


        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> UpdateShipppingSettings(UpdateShippingSettingsModel model)
        {
            var command = new UpdateShippingSettingsCommand
            {
                DefaultShippingSystem = model.DefaultShippingSystem,
                Location = model.Location
            };

            var result = await Send(command);

            return FromResult(result);

        }

    }
}
