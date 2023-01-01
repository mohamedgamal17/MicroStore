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
    public class ShipmentController : MicroStoreApiController
    {

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ShipmentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentModel createShipmentModel)
        {
            var command = new CreateShipmentCommand
            {
                UserId = createShipmentModel.UserId,
                OrderId = createShipmentModel.OrderId,
                Items = createShipmentModel.Items,
                Address = createShipmentModel.Address
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("fullfill/{shipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ShipmentFullfilledDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> FullfillShipment(Guid shipmentId, [FromBody] FullfillShipmentModel model)
        {
            var command = new FullfillShipmentCommand
            {
                ShipmentId = shipmentId,
                SystemName = model.SystemName,
                AddressFrom = model.AddressFrom,
                Pacakge = model.Pacakge
            };

            var result = await Send(command);

            return FromResult(result);
        }


        [HttpPost]
        [Route("estimate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AggregateEstimatedRateCollection))]
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

    }
}
