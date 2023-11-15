using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.ComponentModel.DataAnnotations;
namespace MicroStore.Client.PublicWeb.Pages
{
    [CheckProfilePageCompletedFilter]
    public class ProductDetailsModel : PageModel
    {
        public Product Product { get; set; }
        public List<ProductReview> ProductReviews { get; set; }
        public List<ProductReview> UserReviews { get; set; }

        [BindProperty]
        public CreateProductReviewModel ReviewInput  { get; set; }

        private readonly ProductService _productService;

        private readonly ProductReviewService _productReviewService;

        private readonly ILogger<ProductDetailsModel> _logger;

        private readonly UINotificationManager _uiNotificationManager;

        private readonly IWorkContext _workContext;

        public ProductDetailsModel(ProductReviewService productReviewService, ILogger<ProductDetailsModel> logger, ProductService productService, UINotificationManager uiNotificationManager, IWorkContext workContext)
        {
            _productReviewService = productReviewService;
            _logger = logger;
            _productService = productService;
            _uiNotificationManager = uiNotificationManager;
            _workContext = workContext;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            Product = await _productService.GetAsync(id);

            var productReviewsResponse = await _productReviewService.ListAsync(id, new ProductReviewListRequestOption { Length = 10 });

            var userReveiwsResponse = await _productReviewService
                .ListAsync(id, new ProductReviewListRequestOption { UserId = _workContext.TryToGetCurrentUserId(), Length = 10 });

            ProductReviews = productReviewsResponse.Items;

            UserReviews = userReveiwsResponse.Items;

            return Page();
        }


        public async Task<IActionResult> OnPostSubmitProductReviewAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                _uiNotificationManager.Error( "There is one or more validation error");

                return RedirectToPage("ProductDetails", new { id = id });

            }

            bool isUserAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;

            if (!isUserAuthenticated)
            {
                _uiNotificationManager.Error("You should be logged in to be able to submit your review");

                return RedirectToPage("ProductDetails", new { id = id });
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

                _uiNotificationManager.Success( "Your review has been submited successfully");

               return RedirectToPage("ProductDetails", new { id = id });

            }
            catch(MicroStoreClientException ex) 
            {
                _uiNotificationManager.Error(ex.Erorr.Detail);

                return RedirectToPage("ProductDetails", new { id = id });
            }
            catch(Exception ex)
            {
                _logger.LogException(ex);

                _uiNotificationManager.Error("Sorry service is not available now . Try again later");

                return RedirectToPage("ProductDetails", new { id = id });
            }
        }
    }

    public class CreateProductReviewModel
    {
        [Range(1,5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(265)]
        [MinLength(3)]
        public string ReviewTitle { get; set; }

        [Required]
        [MaxLength(265)]
        [MinLength(3)]
        public string ReviewText { get; set; }
    }
}
