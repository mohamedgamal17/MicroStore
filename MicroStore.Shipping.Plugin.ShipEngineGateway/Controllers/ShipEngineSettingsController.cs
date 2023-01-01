using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ShipEngineSettings>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> GetShipEngineSettings()
        {
            var settings = await _settingsRepository.TryToGetSettings<ShipEngineSettings>(ShipEngineConst.SystemName) ?? new ShipEngineSettings();

            return Success(StatusCodes.Status200OK, settings);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ShipEngineSettings>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> UpdateShipEngineSettings(ShipEngineSettings settings)
        {
            await _settingsRepository.TryToUpdateSettrings(settings);

            return Success(StatusCodes.Status202Accepted, settings);
        }

        [NonAction]
        protected IActionResult Success<TResult>(int statusCode, TResult result)
        {
            return StatusCode(statusCode, Envelope.Success(result));
        }

        [NonAction]
        protected IActionResult Failure(int statusCode, ErrorInfo error)
        {
            return StatusCode(statusCode, Envelope.Failure(error));
        }
    }
}
