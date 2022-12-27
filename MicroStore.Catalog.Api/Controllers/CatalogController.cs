using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using Volo.Abp;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.Catalog.Api.Controllers
{
    [RemoteService(Name = "Catalog")]
    [Route("api/[controller]")]

    public class CatalogController : MicroStoreApiController
    {


        [Route("category")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(Envelope<List<CategoryListDto>>)))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetCatalogCategoryList()
        {
            var request = new GetCategoryListQuery();

            var result = await Send(request);

            return FromResult(result);
        }


        [Route("category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(Envelope<CategoryDto>)))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetCatalogCategory(Guid id)
        {
            var request = new GetCategoryQuery()
            {
                Id = id
            };

            var result = await Send(request);

            return FromResult(result);
        }

        [Authorize]
        [Route("product")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Envelope<List<ProductDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCatalogProductList()
        {
            var request = new GetProductListQuery();

            var result = await Send(request);

            return FromResult(result);
        }


        [Route("product/{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = (typeof(Envelope<ProductDto>)))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCatalogProduct(Guid id)
        {
            var query = new GetProductQuery() { Id = id };

            var result = await Send(query);

            return FromResult(result);
        }
    }
}
