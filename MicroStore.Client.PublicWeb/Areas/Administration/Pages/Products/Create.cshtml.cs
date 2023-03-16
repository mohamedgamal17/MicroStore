using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Products
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ProductModel ProductModel { get; set; } = new ProductModel();
        public List<SelectListItem> CategorySelectItems { get; set; }

        private readonly ProductService _productService;

        private readonly CategoryService _categoryService;

        public CreateModel(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            await BuildViewModel();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await BuildViewModel();

                return Page();
            }

            var productCreateOptions = BuildProductCreateRequest();

            try
            {
                await _productService.CreateAsync(productCreateOptions);

                return RedirectToPage("Index");

            }catch(MicroStoreClientException ex)
            {
                if(ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("", ex.Erorr.Title);
                    ModelState.AddModelError("", ex.Erorr.Detail);
                }

                throw ex;
            }
        }

        private async Task BuildViewModel()
        {
            var categories = await _categoryService.ListAsync(new SortingRequestOptions { Desc  = true});

            if (categories != null)
            {
                CategorySelectItems = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
            }
        }
        private ProductRequestOptions BuildProductCreateRequest()
        {
            var productCreateOptions = new ProductRequestOptions
            {
                Name = ProductModel.Name,
                Sku = ProductModel.Sku,
                ShortDescription = ProductModel.ShortDescription,
                LongDescription = ProductModel.LongDescription,
                Price = ProductModel.Price,
                OldPrice = ProductModel.Price,
                CategoriesIds = ProductModel.CategoriesId
            };

            if (ProductModel.Dimension != null)
            {
                productCreateOptions.Dimensions = new Dimension
                {
                    Lenght = ProductModel.Dimension.Lenght,
                    Height = ProductModel.Dimension.Height,
                    Width = ProductModel.Dimension.Width,
                    Unit = ProductModel.Dimension.Unit,
                };
            }

            if (ProductModel.Weight != null)
            {
                productCreateOptions.Weight = new Weight
                {
                    Value = ProductModel.Weight.Value,
                    Unit = ProductModel.Weight.Unit,
                };
            }

            return productCreateOptions;
        }
    }
}
