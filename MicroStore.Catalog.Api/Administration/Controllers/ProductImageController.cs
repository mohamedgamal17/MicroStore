using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Administration.Models.Products;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
namespace MicroStore.Catalog.Api.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : MicroStoreApiController
    {

        [Route("{productId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid productId, [FromBody] AssignProductImageModel model)
        {
            var command = new AssignProductImageCommand
            {

                ProductId = productId,
                ImageModel = new ImageModel
                {
                    FileName = model.Image.FileName,
                    Type = model.Image.FileName.Split(".")[1],
                    Data = model.Image.GetAllBytes(),
                },
                DisplayOrder = model.DisplayOrder
            };

            var result = await Send(command);

            return FromResult(result);
        }

        [Route("{productId}/{productImageId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [Route("{productId}/{productImageId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
