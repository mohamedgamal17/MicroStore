using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Rates;
using MicroStore.Shipping.Domain.Security;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
    [Authorize(Policy = ApplicationPolicies.RequireAuthenticatedUser)]
    [Route("api/rates")]
    public class ShipmentRateController : MicroStoreApiController
    {
        private readonly IRateApplicationService _rateApplicationService;

        public ShipmentRateController(IRateApplicationService rateApplicationService)
        {
            _rateApplicationService = rateApplicationService;
        }

        [HttpPost]
        [Route("estimate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EstimatedRateDto>))]
        public async Task<IActionResult> EstimateShipmentRate([FromBody]EstimatedRateModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _rateApplicationService.EstimateRate(model);

            return result.ToOk();
        }

    }
}
