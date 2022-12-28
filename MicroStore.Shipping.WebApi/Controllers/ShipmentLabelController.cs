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
    [Route("api/[controller]")]
    public class ShipmentLabelController : MicroStoreApiController
    {
        [HttpPost]
        [Route("estimate")]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type = typeof(AggregateEstimatedRateCollection))]
        public async Task<IActionResult> EstimateShipmentRate(EstimateShipmentRateModel model)
        {
            var command = new EstimateShipmentRateCommand
            {
                Address = model.Address,
                Items = model.Items
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("rates")]
        [ProducesResponseType(StatusCodes.Status202Accepted,Type = typeof(Envelope<List<ShipmentRateDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveShipmentRates([FromBody] RetriveShipmentRateModel model)
        {
            var command = new RetriveShipmentRateCommand
            {
                SystemName = model.SystemName,
                ExternalShipmentId = model.ExternalShipmentId
            };

            var result= await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("buylabel")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<List<ShipmentDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope))]
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
