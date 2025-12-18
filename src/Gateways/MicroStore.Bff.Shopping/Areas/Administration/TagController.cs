using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Models.Catalog.ProductTags;
using MicroStore.Bff.Shopping.Services.Catalog;
namespace MicroStore.Bff.Shopping.Areas.Administration
{
    [ApiExplorerSettings(GroupName = "Administration")]
    [Route("api/administration/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ProductTagService _productTagService;

        public TagController(ProductTagService productTagService)
        {
            _productTagService = productTagService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductTag))]
        public async Task<List<ProductTag>> ListAsync()
        {
            var tags = await _productTagService.ListAsync();

            return tags;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductTag))]
        public async Task<ProductTag> GetAsync(string id)
        {
            var tag = await _productTagService.GetAsync(id);

            return tag;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductTag))]
        public async Task<ProductTag> CreateAsync(ProductTagModel model)
        {
            var tag = await _productTagService.CreateAsync(model);

            return tag;
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductTag))]
        public async Task<ProductTag> UpdateAsync(string id, ProductTagModel model)
        {
            var tag = await _productTagService.UpdateAsync(id, model);

            return tag;
        }
    }
}
