using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Addresses;
using System.Net;

namespace MicroStore.Shipping.WebApi.Controllers
{
    [ApiController]
   // [Authorize]
    [Route("api/addresses")]
    public class AddressController : MicroStoreApiController
    {
        private readonly IAddressApplicationService _addressApplicationService;

        public AddressController(IAddressApplicationService addressApplicationService)
        {
            _addressApplicationService = addressApplicationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(AddressValidationResultModel))]
        public async Task<IActionResult> ValidateAddress([FromBody] AddressModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _addressApplicationService.ValidateAddress(model);

            return result.ToOk();
        }

    }
}
