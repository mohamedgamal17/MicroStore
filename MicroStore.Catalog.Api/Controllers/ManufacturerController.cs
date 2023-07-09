using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Api.Infrastructure;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Manufacturers;
using MicroStore.Catalog.Application.Models.Manufacturers;
namespace MicroStore.Catalog.Api.Controllers
{

    [Route("api/manufacturers")]
    [ApiController]

    public class ManufacturerController : MicroStoreApiController
    {
        private readonly IManufacturerCommandService _manufacturerCommandService;

        private readonly IManufacturerQueryService _manufacturerQueryService;

        public ManufacturerController(IManufacturerCommandService manufacturerCommandService, IManufacturerQueryService manufacturerQueryService)
        {
            _manufacturerCommandService = manufacturerCommandService;
            _manufacturerQueryService = manufacturerQueryService;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(List<ManufacturerDto>)))]
        public async Task<IActionResult> GetManufacturerList([FromQuery] SortingQueryParams queryParams)
        {
            var result = await _manufacturerQueryService
                .ListAsync(queryParams);

            return result.ToOk();
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(ManufacturerDto)))]

        public async Task<IActionResult> GetManufacturer(string id)
        {
            var result = await _manufacturerQueryService.GetAsync(id);

            return result.ToOk();
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = (typeof(ManufacturerDto)))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> CreateManufacturer(ManufacturerModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _manufacturerCommandService.CreateAsync(model);

            return result.ToCreatedAtAction("GetManufacturerList", new { id = result.Value?.Id });
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(ManufacturerDto)))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]

        public async Task<IActionResult> UpdateManufacturer(string id , ManufacturerModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();  
            }
            var result = await _manufacturerCommandService.UpdateAsync(id,model);

            return result.ToOk();
        }

    }
}
