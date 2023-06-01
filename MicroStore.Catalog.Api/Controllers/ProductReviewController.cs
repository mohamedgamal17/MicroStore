using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.ProductReviews;
using MicroStore.Catalog.Application.ProductReviews;
namespace MicroStore.Catalog.Api.Controllers
{
    [Route("api/products/{productId}/productreviews")]
    public class ProductReviewController : MicroStoreApiController
    {

        private readonly IProductReviewService _productReviewService;

        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }


        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<ProductReviewDto>))]
        public async Task<IActionResult> ListProductReview(string productId ,[FromQuery] PagingParamsQueryString queryParams)
        {
            var result = await _productReviewService.ListAsync(productId, new PagingQueryParams
            {
                Skip = queryParams.Skip,
                Lenght = queryParams.Lenght
            });

            return result.ToOk();
        }

        [Route("{productReviewId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        public async Task<IActionResult> GetProductReview(string productId, string productReviewId)
        {
            var result = await _productReviewService.GetAsync(productId, productReviewId);

            return result.ToOk();
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        public async Task<IActionResult> CreateProductReview(string productId ,[FromBody] CreateProductReviewModel model)
        {
            var result = await _productReviewService.CreateAsync(productId, model);

            return result.ToCreatedAtAction(nameof(GetProductReview), 
                new { productId = result.Value?.Id , productReviewId = result.Value?.ProductId });
        }

        [Route("{productReviewId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        public async Task<IActionResult> UpdateProductReview(string productId ,string productReviewId ,[FromBody] ProductReviewModel model)
        {
            var result = await _productReviewService.UpdateAsync(productId, productReviewId, model);

            return result.ToOk();
        }

        [Route("{productReviewId}/replay")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        public async Task<IActionResult> ReplayOnProductReview(string productId , string productReviewId, [FromBody] ProductReviewReplayModel model)
        {
            var result = await _productReviewService.ReplayAsync(productId, productReviewId, model);

            return result.ToOk();
        }

        [Route("{productReviewId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        public async Task<IActionResult> RemvoeProductReview(string productId , string productReviewId)
        {
            var result = await _productReviewService.DeleteAsync(productId, productReviewId);

            return result.ToOk();
        }

    }
}
