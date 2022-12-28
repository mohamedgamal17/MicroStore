using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.WebApi.Models;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Route("updatelocation")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type= typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type= typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type= typeof(Envelope))]
        public async Task<IActionResult> UpdateShipmentLocation(UpdateShippingLocationModel model)
        {
            var command = new UpdateShippingLocationCommand
            {
                Name = model.Name,
                CountryCode = model.CountryCode,
                State = model.State,
                City = model.City,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                PostalCode = model.PostalCode,
                Zip = model.Zip
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
