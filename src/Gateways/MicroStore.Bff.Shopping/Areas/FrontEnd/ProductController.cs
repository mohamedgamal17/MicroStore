using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Services.Catalog;

namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Product>))]
        public async Task<PagedList<Product>> ListAsync(string name = "", string categories = "", string manufacturers = "", string tags = "", bool isFeatured = false, double minPrice = -1, double maxPrice = -1, int length = 10, int skip = 0, string sortBy = "")
        {
            var result = await _productService.ListAsync(name, categories, manufacturers, tags, isFeatured, minPrice, maxPrice, skip, length, sortBy);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<Product> GetAsync(string id)
        {
            var result = await _productService.GetAsync(id);

            return result;
        }


        [HttpGet]
        [Route("{productId}/images")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductImage>))]
        public async Task<List<ProductImage>> ListProductImagesAsync(string productId)
        {
            var result = await _productService.ListProductImageAsync(productId);

            return result;
        }

        [HttpGet]
        [Route("{productId}/images/{imageId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductImage))]
        public async Task<ProductImage> GetProductImageAsync(string productId, string imageId)
        {
            var result = await _productService.GetProductImageAsync(productId, imageId);

            return result;
        }
    }
}
