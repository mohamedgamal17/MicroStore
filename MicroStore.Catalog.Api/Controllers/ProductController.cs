using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Models.Products;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;

namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : MicroStoreApiController
    {
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PagedResult<ProductListDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCatalogProductList(PagingAndSortingQueryParams @params)
        {
            var request = new GetProductListQuery
            {
                SortBy = @params.SortBy,
                Desc = @params.Desc,
                PageSize = @params.PageSize,
                PageNumber = @params.PageNumber,
            };

            var result = await Send(request);

            return FromResult(result);
        }


        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(Envelope<ProductDto>)))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCatalogProduct(Guid id)
        {
            var query = new GetProductQuery() { Id = id };

            var result = await Send(query);

            return FromResult(result);
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateProductModel model)
        {
            CreateProductCommand command = new CreateProductCommand
            {
                Name = model.Name,
                Sku = model.Sku,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,

                OldPrice = model.OldPrice,
                Price = model.Price,

                Thumbnail = model.Thumbnail,

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
                Thumbnail = model.Thumbnail,
                Dimensions = model.Dimensions,
                Weight = model.Weight
            };

            var result = await Send(command);

            return FromResult(result);
        }

    }
}
