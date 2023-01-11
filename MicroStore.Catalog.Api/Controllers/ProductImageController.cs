using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Models.Products;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Security;

namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/products/{productId}/productimages")]
    [Authorize]
    [ApiController]
    public class ProductImageController : MicroStoreApiController
    {

        [Route("")]
        [HttpPost]
        [RequiredScope(CatalogScope.ProductImage.Create)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> Post(Guid productId, [FromBody] AssignProductImageModel model)
        {
            var command = new AssignProductImageCommand
            {

                ProductId = productId,
                ImageModel = model.Image,
                DisplayOrder = model.DisplayOrder
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [Route("{productImageId}")]
        [HttpPut]
        [RequiredScope(CatalogScope.ProductImage.Update)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid productId, Guid productImageId, [FromBody] UpdateProductImageModel model)
        {
            var command = new UpdateProductImageCommand
            {
                ProductId = productId,
                ProductImageId = productImageId,
                DisplayOrder = model.DisplayOrder
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [Route("{productImageId}")]
        [HttpDelete]
        [RequiredScope(CatalogScope.ProductImage.Delete)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid productId, Guid productImageId)
        {
            var command = new RemoveProductImageCommand
            {
                ProductId = productId,
                ProductImageId = productImageId
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }
}
