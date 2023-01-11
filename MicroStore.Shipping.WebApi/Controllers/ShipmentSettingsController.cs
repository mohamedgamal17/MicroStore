using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Queries;
using MicroStore.Shipping.Domain.Security;
using MicroStore.Shipping.WebApi.Models.Settings;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class ShipmentSettingsController : MicroStoreApiController
    {

        [HttpGet]
        [Route("")]
        [RequiredScope(ShippingScope.Settings.Read)]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(Envelope))]
        public async Task<IActionResult> GetShipmentSettings()
        {
            var query = new GetShippingSettingsQuery();

            var result = await Send(query);

            return FromResult(result);
        }


        [HttpPost]
        [Route("")]
        [RequiredScope(ShippingScope.Settings.Update)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> UpdateShipppingSettings([FromBody]UpdateShippingSettingsModel model)
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
