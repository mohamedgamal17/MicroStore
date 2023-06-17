using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Catalog.Application.Models.SpecificationAttributes;
using MicroStore.Catalog.Application.SpecificationAttributes;
namespace MicroStore.Catalog.Api.Controllers
{

    [ApiController]
    [Route("api/specificationattributes")]
    public class SpecificationAttributeController : MicroStoreApiController
    {

        private readonly ISpecificationAttributeApplicationService _specificationAttributeApplicationService;

        public SpecificationAttributeController(ISpecificationAttributeApplicationService specificationAttributeApplicationService)
        {
            _specificationAttributeApplicationService = specificationAttributeApplicationService;
        }

        [HttpGet]
        public  async Task<IActionResult> ListAsync()
        {
            var result =  await _specificationAttributeApplicationService.ListAsync();

            return result.ToOk();
        }


        [HttpGet]
        [Route("{attributeId}")]
        public async Task<IActionResult> GetAsync(string attributeId)
        {
            var result = await _specificationAttributeApplicationService.GetAsync(attributeId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(SpecificationAttributeModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }


            var result = await _specificationAttributeApplicationService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetAsync), new { attributeId = result.Value?.Id });
        }

        [HttpPut]
        [Route("{attributeId}")]
        public async Task<IActionResult> UpdateAsync(string attributeId , SpecificationAttributeModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _specificationAttributeApplicationService.UpdateAsync(attributeId,model);

            return result.ToOk();
        }

        [HttpDelete]
        [Route("{attributeId}")]
        public async Task<IActionResult> RemoveAsync(string attributeId)
        {
            var result = await _specificationAttributeApplicationService.RemoveAsync(attributeId);

            return result.ToOk();
        }



        [HttpGet]
        [Route("{attributeId}/options")]
        public async Task<IActionResult> ListOptionsAsync(string attributeId)
        {
            var result = await _specificationAttributeApplicationService.ListOptionsAsync(attributeId);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{attributeId}/options/{optionId}")]
        public async Task<IActionResult> GetOptionAsync(string attributeId , string optionId)
        {
            var result=  await _specificationAttributeApplicationService.GetOptionAsync(attributeId, optionId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("{attributeId}/options")]
        public async Task<IActionResult> CreateOptionAsync(string attributeId, SpecificationAttributeOptionModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _specificationAttributeApplicationService.CreateOptionAsync(attributeId, model);

            return result.ToOk();
        }

        [HttpPut]
        [Route("{attributeId}/options/{optionId}")]
        public async Task<IActionResult> UpdateOptionAsync(string attributeId, string optionId, SpecificationAttributeOptionModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _specificationAttributeApplicationService.UpdateOptionAsync(attributeId, optionId ,model);

            return result.ToOk();
        }

        [HttpDelete]
        [Route("{attributeId}/options/{optionId}")]
        public async Task<IActionResult> RemoveOptionAsync(string attributeId, string optionId)
        {
            var result = await _specificationAttributeApplicationService.RemoveOptionAsync(attributeId, optionId);

            return result.ToOk();
        }
    }
}
