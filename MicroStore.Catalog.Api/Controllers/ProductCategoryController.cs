using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Models.Products;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Security;

namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/products/{productId}/productcategories")]
    [Authorize]
    [ApiController]
    public class ProductCategoryController : MicroStoreApiController
    {
        private readonly ILocalMessageBus _localMessageBus;

        public ProductCategoryController(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }

        [Route("")]
        [HttpPost]
        [RequiredScope(CatalogScope.ProductCategory.Create)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid productId, [FromBody] AssignProductCategoryModel model)
        {
            var command = new AssignProductCategoryCommand
            {
                ProductId = productId,
                CategoryId = model.CategoryId,
                IsFeatured = model.IsFeatured,
            };

            var result = await _localMessageBus.Send(command);

            return FromResult(result);
        }

        [Route("{categoryId}")]
        [HttpPut]
        [RequiredScope(CatalogScope.ProductCategory.Update)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid productId, Guid categoryId, [FromBody] UpdateProductCategoryModel model)
        {
            var command = new UpdateProductCategoryCommand
            {
                ProductId = productId,
                CategoryId = categoryId,
                IsFeatured = model.IsFeatured
            };

            var result = await _localMessageBus.Send(command);

            return FromResult(result);
        }

        [Route("{categoryId}")]
        [HttpDelete]
        [RequiredScope(CatalogScope.ProductCategory.Delete)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid productId, Guid categoryId)
        {
            var command = new RemoveProductCategoryCommand
            {
                CategoryId = categoryId,
                ProductId = productId
            };

            var result = await _localMessageBus.Send(command);

            return FromResult(result);
        }
    }
}
