using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Api.Infrastructure;
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
        public async Task<IActionResult> ListProductReview(string productId ,[FromQuery] ProductReviewListQueryModel queryParams)
        {
            var result = await _productReviewService.ListAsync(productId, queryParams);

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
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> CreateProductReview(string productId ,[FromBody] ProductReviewModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var createModel = new CreateProductReviewModel
            {
                Title = model.Title,
                Rating = model.Rating,
                ReviewText = model.ReviewText,
                UserId = CurrentUser.Id.ToString()!
            };

            var result = await _productReviewService.CreateAsync(productId, createModel);

            return result.ToCreatedAtAction(nameof(GetProductReview), 
                new { productId = result.Value?.Id , productReviewId = result.Value?.ProductId });
        }

        [Route("{productReviewId}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> UpdateProductReview(string productId ,string productReviewId ,[FromBody] ProductReviewModel model)
        {
            var result = await _productReviewService.UpdateAsync(productId, productReviewId, model);

            return result.ToOk();
        }

        [Route("{productReviewId}/replay")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> ReplayOnProductReview(string productId , string productReviewId, [FromBody] ProductReviewReplayModel model)
        {
            var result = await _productReviewService.ReplayAsync(productId, productReviewId, model);

            return result.ToOk();
        }

        [Route("{productReviewId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        [Authorize(Policy = ApplicationAuthorizationPolicy.RequeireAuthenticatedUser)]
        public async Task<IActionResult> RemvoeProductReview(string productId , string productReviewId)
        {
            var result = await _productReviewService.DeleteAsync(productId, productReviewId);

            return result.ToOk();
        }

    }
}
