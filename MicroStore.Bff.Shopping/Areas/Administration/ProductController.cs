using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Models.Catalog.Products;
using MicroStore.Bff.Shopping.Services.Catalog;

namespace MicroStore.Bff.Shopping.Areas.Administration
{
    [ApiExplorerSettings(GroupName = "Administration")]
    [Route("api/administration/products")]
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
        public async Task<PagedList<Product>> ListAsync(string name = "", string categories = "", string manufacturers = "", string tags = "", bool isFeatured = false, double minPrice = -1, double maxPrice = -1,
            int skip = 0, int length = 10, string sortBy = "", bool desc = false)
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


        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<Product> CreateAsync(ProductModel model)
        {
            var result = await _productService.CreateAsync(model);

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<Product> UpdateAsync(string id, ProductModel model)
        {
            var result = await _productService.UpdateAsync(id, model);

            return result;
        }

        [HttpGet]
        [Route("/{productId}/images")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductImage>))]
        public async Task<List<ProductImage>> ListProductImagesAsync(string productId)
        {
            var result = await _productService.ListProductImageAsync(productId);

            return result;
        }

        [HttpGet]
        [Route("/{productId}/images/{imageId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductImage))]
        public async Task<ProductImage> GetProductImageAsync(string productId, string imageId)
        {
            var result = await _productService.GetProductImageAsync(productId, imageId);

            return result;
        }

        [HttpPost]
        [Route("/{productId}/images")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductImage))]
        public async Task<ProductImage> CreateProductImageAsync(string productId, ProductImageModel model)
        {
            var result = await _productService.CreateProductImageAsync(productId, model);

            return result;
        }

        [HttpPut]
        [Route("/{productId}/images/imageId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductImage))]
        public async Task<ProductImage> UpdateAsync(string productId, string imageId, ProductImageModel model)
        {
            var result = await _productService.UpdateProductImageAsync(productId, imageId, model);

            return result;
        }

        [HttpDelete]
        [Route("/{productId}/images/imageId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(string productId, string imageId)
        {
            await _productService.DeleteProductImageAsync(productId, imageId);

            return NoContent();
        }
    }
}
