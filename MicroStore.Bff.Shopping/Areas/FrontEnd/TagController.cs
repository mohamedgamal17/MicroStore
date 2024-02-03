using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Services.Catalog;

namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/tags")]
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
    }
}
