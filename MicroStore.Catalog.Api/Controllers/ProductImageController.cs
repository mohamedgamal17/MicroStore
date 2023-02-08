using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Models;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Products;

namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/products/{productId}/productimages")]
    [ApiController]
    public class ProductImageController : MicroStoreApiController
    {

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Envelope))]
        public async Task<IActionResult> CreateProductImage(Guid productId, [FromBody] ProductImageModel model)
        {
            var command = ObjectMapper.Map<ProductImageModel, CreateProductImageCommand>(model);

            command.ProductId = productId;

            var result = await Send(command);

            return FromResult(result);
        }

        [Route("{productImageId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid productId, Guid productImageId, [FromBody] ProductImageModel model)
        {
            var command = ObjectMapper.Map<ProductImageModel, UpdateProductImageCommand>(model);

            command.ProductId = productId;
            command.ProductImageId = productImageId;

            var result = await Send(command);

            return FromResult(result);
        }

        [Route("{productImageId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<ProductDto>))]
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
