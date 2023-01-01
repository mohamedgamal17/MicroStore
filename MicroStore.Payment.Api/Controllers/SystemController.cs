using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Api.Models;
using MicroStore.Payment.Application.Abstractions.Commands;

namespace MicroStore.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/systems")]
    public class SystemController : MicroStoreApiController
    {



        [HttpPut]
        [Route("{systemName}")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> UpdatePluginSystem(string systemName,[FromBody] UpdatePluginSystemModel model)
        {
            var command = new UpdatePaymentSystemCommand
            {
                IsEnabled = model.IsEnabled,
            };

            var result = await Send(command);

            return FromResult(result);
        }

    }
}
