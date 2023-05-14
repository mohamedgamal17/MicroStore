using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Products;
namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/products")]
   // [Authorize]
    [ApiController]
    public class ProductController : MicroStoreApiController
    {

        private readonly IProductCommandService _productCommandService;

        private readonly IProductQueryService _productQueryService;

        public ProductController(IProductCommandService productCommandService, IProductQueryService productQueryService)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ProductDto>))]
        public async Task<IActionResult> GetCatalogProductList([FromQuery] PagingAndSortingParamsQueryString queryParams)
        {
            var result = await _productQueryService.ListAsync(new PagingAndSortingQueryParams
            {
                Skip = queryParams.Skip,
                Lenght = queryParams.Lenght,
                SortBy = queryParams.SortBy,
                Desc = queryParams.Desc
            });


            return result.ToOk();

        }


        [Route("{id}")]
        [ActionName(nameof(GetCatalogProduct))]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(ProductDto)))]
        public async Task<IActionResult> GetCatalogProduct(string id)
        {
            var result = await _productQueryService.GetAsync(id);

            return result.ToOk();
        }

        [Route("")]
        [ActionName(nameof(CreateProduct))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productCommandService.CreateAsync(model);

            return result.ToCreatedAtAction(nameof(GetCatalogProduct),new {id = result.Value?.Id});
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productCommandService.UpdateAsync(id, model);

            return result.ToOk();
        }

        [Route("{productId}/productimages")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<ProductImageDto>>))]
        public async Task<IActionResult> GetProductImagesAsync(string productId)
        {
            var result = await _productQueryService.ListProductImagesAsync(productId);

            return result.ToOk();
        }


        [Route("{productId}/productimages")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<IActionResult> AddProductImage (string productId, [FromBody] CreateProductImageModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productCommandService.AddProductImageAsync(productId, model);

            return result.ToOk();

        }


        [Route("{productId}/productimages/{productImageId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<IActionResult> UpdateProductImage(string productId, string productImageId,[FromBody] UpdateProductImageModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productCommandService.UpdateProductImageAsync(productId, productImageId,model);
            return result.ToOk();
        }

        [Route("{productId}/productimages/{productImageId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<IActionResult> DeleteProductImage(string productId, string productImageId)
        {
            var result = await _productCommandService.DeleteProductImageAsync(productId, productImageId);

            return result.ToOk();
        }
    }
}
