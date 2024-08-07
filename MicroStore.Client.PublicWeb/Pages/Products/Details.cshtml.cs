using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Pages.Products
{
    [CheckProfilePageCompletedFilter]
    public class DetailsModel : PageModel
    {
        public Product Product { get; set; }
        public List<ProductReviewAggregate> ProductReviews { get; set; }
        public List<ProductReviewAggregate> UserReviews { get; set; }

        [BindProperty]
        public CreateProductReviewModel ReviewInput { get; set; }

        private readonly ProductService _productService;

        private readonly ProductReviewService _productReviewService;

        private readonly ProductReviewAggregateService _productReviewAggregateService;

        private readonly ILogger<DetailsModel> _logger;

        private readonly UINotificationManager _uiNotificationManager;

        private readonly IWorkContext _workContext;

        public DetailsModel(ProductService productService, ProductReviewService productReviewService, ProductReviewAggregateService productReviewAggregateService, ILogger<DetailsModel> logger, UINotificationManager uiNotificationManager, IWorkContext workContext)
        {
            _productService = productService;
            _productReviewService = productReviewService;
            _productReviewAggregateService = productReviewAggregateService;
            _logger = logger;
            _uiNotificationManager = uiNotificationManager;
            _workContext = workContext;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            Product = await _productService.GetAsync(id);

            var productReviewsResponse = await _productReviewAggregateService.ListAsync(id, new ProductReviewListRequestOption { Length = 10 });

            var userReveiwsResponse = await _productReviewAggregateService
                .ListAsync(id, new ProductReviewListRequestOption { UserId = _workContext.TryToGetCurrentUserId(), Length = 10 });

            ProductReviews = productReviewsResponse.Items;

            UserReviews = userReveiwsResponse.Items;

            return Page();
        }


        public async Task<IActionResult> OnPostSubmitProductReviewAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                _uiNotificationManager.Error("There is one or more validation error");

                return RedirectToPage("Details", new { id = id });

            }

            bool isUserAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;

            if (!isUserAuthenticated)
            {
                _uiNotificationManager.Error("You should be logged in to be able to submit your review");

                return RedirectToPage("Details", new { id = id });
            }

            var requestOptions = new ProductReviewRequestOption
            {
                Title = ReviewInput.ReviewTitle,
                ReviewText = ReviewInput.ReviewText,
                Rating = ReviewInput.Rating
            };

            try
            {
                var response = await _productReviewService.CreateAsync(id, requestOptions);

                _uiNotificationManager.Success("Your review has been submited successfully");

                return RedirectToPage("Details", new { id = id });

            }
            catch (MicroStoreClientException ex) when(ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                _uiNotificationManager.Error(ex.Error.Detail);

                return RedirectToPage("Details", new { id = id });
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);

                _uiNotificationManager.Error("Sorry service is not available now . Try again later");

                return RedirectToPage("Details", new { id = id });
            }
        }
    }
}
