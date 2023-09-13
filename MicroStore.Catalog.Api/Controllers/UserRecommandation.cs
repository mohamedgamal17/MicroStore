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
    [Route("api/user-recommandation")]
    [ApiController]
    public class UserRecommandation : MicroStoreApiController
    {
        private readonly IProductQueryService _productQueryService;

        public UserRecommandation(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        [Route("products")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ElasticProduct>))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> GetProductRecommendation(PagingQueryParams queryParams)
        {
            var result = await _productQueryService.GetUserRecommendation(CurrentUser.Id.ToString()!, queryParams);

            return result.ToOk();
        }

    }
}
