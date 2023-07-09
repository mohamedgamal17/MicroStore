﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Settings;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.Security;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class ShipmentSettingsController : MicroStoreApiController
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        public ShipmentSettingsController(IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(ShippingSettings))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]

        public async Task<IActionResult> GetShipmentSettings()
        {
            var result = await _applicationSettingsService.GetAsync();

            return Ok(result);
        }


        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShippingSettings))]
        [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]

        public async Task<IActionResult> UpdateShipppingSettings([FromBody]UpdateShippingSettingsModel model)
        {
            var validationResult = await ValidateModel(model);

            if(!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _applicationSettingsService.UpdateAsync(model);

            return Ok(result);

        }

    }
}
