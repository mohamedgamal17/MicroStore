using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Api.Models;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : MicroStoreApiController
    {

        [HttpPut]
        [Route("adjustQuantity/{productsku}")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductAdjustedInventoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> AdjustProductInventory(string productsku, AdjustProductInventoryModel model)
        {
            var command = new AdjustProductInventoryCommand
            {
                Sku = productsku,
                Stock = model.AdjustedQuantity,
                Reason = model.Reason
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
