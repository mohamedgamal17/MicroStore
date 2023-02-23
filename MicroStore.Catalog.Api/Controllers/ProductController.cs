using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Products;
using System.Net;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ProductListDto>))]
        public async Task<IActionResult> GetCatalogProductList([FromQuery] PagingAndSortingParamsQueryString queryParams)
        {
            var result = await _productQueryService.ListAsync(new PagingAndSortingQueryParams
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                SortBy = queryParams.SortBy,
                Desc = queryParams.Desc
            });


            return FromResult(result,HttpStatusCode.OK);
        }


        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(ProductDto)))]
        public async Task<IActionResult> GetCatalogProduct(string id)
        {
            var result = await _productQueryService.GetAsync(id);

            return FromResult(result, HttpStatusCode.OK);
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        public async Task<IActionResult> Post([FromBody] ProductModel model)
        {
            var result = await _productCommandService.CreateAsync(model);

            return FromResult(result,HttpStatusCode.Created);
        }

        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<IActionResult> Put(string id, [FromForm] ProductModel model)
        {
            var result = await _productCommandService.UpdateAsync(id, model);
            return FromResult(result, HttpStatusCode.Created);
        }

    }
}
