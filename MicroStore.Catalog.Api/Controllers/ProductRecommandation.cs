using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Api.Infrastructure;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/product-recommandation")]
    [ApiController]
    public class ProductRecommandation : MicroStoreApiController
    {
        private readonly IProductQueryService _productQueryService;

        public ProductRecommandation(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        [Route("user-recommandation")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ElasticProduct>))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> GetProductRecommendation([FromQuery] PagingQueryParams queryParams)
        {
            var result = await _productQueryService.GetUserRecommendation(CurrentUser.UserId!, queryParams);

            return result.ToOk();
        }

        [Route("similar-items/{productId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ElasticProduct>))]
        public async Task<IActionResult> GetSimilarItems(string productId , [FromQuery] PagingQueryParams queryParams)
        {
            var result = await _productQueryService.GetSimilarItems(productId, queryParams);

            return result.ToOk();
        }
    }
}
