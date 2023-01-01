using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions.Common;
using MicroStore.Payment.Plugin.StripeGateway.Config;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using MicroStore.Payment.Plugin.StripeGateway.Model;
namespace MicroStore.Payment.Plugin.StripeGateway.Controller
{
    [ApiController]
    [Route("api/plugins/stripesettings")]
    public class StripeSettingsController : ControllerBase
    {
        private readonly ISettingsRepository _settingsRepository;

        public StripeSettingsController(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<StripePaymentSettings>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task <IActionResult> GetStripeSettings()
        {
            var settings = await _settingsRepository.TryToGetSettings<StripePaymentSettings>(StripePaymentConst.Provider);

            return Success(StatusCodes.Status200OK, settings);
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<StripePaymentSettings>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> UpdateStripeSettings(UpdateStripeSettingsModel model)
        {
            var settings = await _settingsRepository
                .TryToGetSettings<StripePaymentSettings>(StripePaymentConst.Provider) ?? new StripePaymentSettings();

            settings.ApiKey = model.ApiKey;
            settings.EndPointSecret = model.EndPointSecret;
            settings.Currency = model.Currency;
            settings.PaymentMethods = model.PaymentMethods;

            await _settingsRepository.TryToUpdateSettrings(settings);

            return Success(StatusCodes.Status202Accepted, settings);
        }


        [NonAction]
        protected IActionResult Success<TResult>(int statusCode,TResult result)
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
