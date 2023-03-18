﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Controllers
{
    [ApiController]
    [Route("api/plugins/shipenginesettings")]
    public class ShipEngineSettingsController : ControllerBase
    {
        private readonly ISettingsRepository _settingsRepository;

        public ShipEngineSettingsController(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShipEngineSettings))]
        public async Task<IActionResult> GetShipEngineSettings()
        {
            var settings = await _settingsRepository.TryToGetSettings<ShipEngineSettings>(ShipEngineConst.SystemName) ?? new ShipEngineSettings();

            return Ok(settings);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(ShipEngineSettings))]
        public async Task<IActionResult> UpdateShipEngineSettings(ShipEngineSettings settings)
        {
            await _settingsRepository.TryToUpdateSettrings(settings);

            return Ok(settings);
        }

       
    }
}
