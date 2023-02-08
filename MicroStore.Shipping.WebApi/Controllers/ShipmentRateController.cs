using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.Security;
using MicroStore.Shipping.WebApi.Models.Rates;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/rates")]
    public class ShipmentRateController : MicroStoreApiController
    {
        [HttpPost]
        [Route("retrive")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<List<ShipmentRateDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope))]
        public async Task<IActionResult> RetriveShipmentRates([FromBody] RetriveShipmentRateModel model)
        {
            var command = new RetriveShipmentRateCommand
            {
                SystemName = model.SystemName,
                ExternalShipmentId = model.ExternalShipmentId
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [HttpPost]
        [Route("estimate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListResultDto<EstimatedRateDto>))]
        public async Task<IActionResult> EstimateShipmentRate([FromBody]EstimateShipmentRateModel model)
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
