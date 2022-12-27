using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Administration.Models.Products;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using Volo.Abp;
namespace MicroStore.Catalog.Api.Administration.Controllers
{
    [RemoteService(Name = "Product")]
    [Area("Administration")]
    [Route("api/[Area]/[Controller]")]
    [ApiController]

    public class ProductController : MicroStoreApiController
    {

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromForm] CreateProductModel model)
        {
            CreateProductCommand command = new CreateProductCommand
            {
                Name = model.Name,
                Sku = model.Sku,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,

                OldPrice = model.OldPrice,
                Price = model.Price,

                ImageModel = new ImageModel
                {
                    FileName = model.Thumbnail?.FileName,
                    Data = model.Thumbnail?.GetAllBytes(),
                    Type = model.Thumbnail?.FileName.Split(".")[1]
                },

                Dimensions = model.Dimensions,

                Weight = model.Weight

            };


            var result = await Send(command);

            return FromResult(result);
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromForm] UpdateProductModel model)
        {

            UpdateProductCommand command = new UpdateProductCommand
            {
                Name = model.Name,
                Sku = model.Sku,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                OldPrice = model.OldPrice,
                Price = model.Price,
                ImageModel = new ImageModel
                {
                    FileName = model.Thumbnail?.FileName,
                    Data = model.Thumbnail?.GetAllBytes(),
                    Type = model.Thumbnail?.FileName.Split(".")[1]
                },
                Dimensions = model.Dimensions,
                Weight = model.Weight
            };

            var result = await Send(command);

            return FromResult(result);
        }
    }

}
