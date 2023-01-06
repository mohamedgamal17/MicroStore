﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.WebApi.Models.Rates;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
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
