using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Catalog.Application.Models.ProductTags;
using MicroStore.Catalog.Application.ProductTags;
namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/producttags")]
    [ApiController]
    public class ProductTagController : MicroStoreApiController
    {
        private readonly IProductTagApplicationService _productTagApplicationService;

        public ProductTagController(IProductTagApplicationService productTagApplicationService)
        {
            _productTagApplicationService = productTagApplicationService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ListAsync()
        {
            var result = await _productTagApplicationService.ListAsync();

            return result.ToOk();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var result = await _productTagApplicationService.GetAsync(id);

            return result.ToOk();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(ProductTagModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productTagApplicationService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetAsync), new {id = result.Value?.Id});
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(string id , ProductTagModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productTagApplicationService.UpdateAsync(id,model);

            return result.ToOk();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _productTagApplicationService.RemoveAsync(id);

            return result.ToNoContent();
        }
    }
}
