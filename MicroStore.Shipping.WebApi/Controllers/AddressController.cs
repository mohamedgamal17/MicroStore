using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.WebApi.Models.Addresses;
namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressController : MicroStoreApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<AddressValidationResultModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type = typeof(Envelope))]
        public async Task<IActionResult> ValidateAddress([FromBody] ValidateAddressModel model)
        {
            var command = new ValidateAddressCommand
            {
                Name = model.Name,
                Phone = model.Phone,
                CountryCode = model.CountryCode,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Zip = model.Zip,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2
            };

            var result = await Send(command);

            return FromResult(result);
        }

    }
}
