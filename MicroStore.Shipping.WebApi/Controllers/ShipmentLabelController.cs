using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.WebApi.Models;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/labels")]
    public class ShipmentLabelController : MicroStoreApiController
    {
      


        [HttpPost]
        [Route("buylabel")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<List<ShipmentDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        public async Task<IActionResult> BuyShipmentLabel([FromBody] MicroStore.Shipping.WebApi.Models.BuyShipmentLabelModel model)
        {
            var command = new BuyShipmentLabelCommand
            {
                SystemName = model.SystemName,
                ExternalShipmentId = model.ExternalShipmentId,
                RateId  = model.ShipmentRateId,

            };

            var result= await Send(command);

            return FromResult(result);
        }
    }
}
