using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Api.Models;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Products;

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
        public async Task<IActionResult> GetCatalogProductList([FromQuery] PagingAndSortingParamsQueryString @params)
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ProductModel model)
        {
            CreateProductCommand command = ObjectMapper.Map<ProductModel, CreateProductCommand>(model);


            var result = await Send(command);

            return FromResult(result);
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Envelope<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, [FromForm] ProductModel model)
        {
            UpdateProductCommand command = ObjectMapper.Map<ProductModel, UpdateProductCommand>(model);

            command.ProductId = id;

            var result = await Send(command);

            return FromResult(result);
        }

    }
}
