using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Common;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Inventory;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Inventory;
using NUglify.Helpers;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Products
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ProductModel ProductModel { get; set; } = new ProductModel();

        [BindProperty]
        public InventoryItemModel InventoryItemModel { get; set; }

        public List<SelectListItem> CategorySelectItems { get; set; }

        private readonly CategoryService _categoryService;

        private readonly ProductService _productService;


        public EditModel(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;

        }

        public async Task<IActionResult> OnGet(string productId)
        {
            await PrepareProductModel(productId);

            await BuildViewModel();

            return Page();
        }

        public async Task<IActionResult> OnPost(string productId)
        {
            if (!ModelState.IsValid)
            {
                await PrepareProductModel(productId);


                await BuildViewModel();


                return Page();
            }

            var productRequestOptions = BuildProductRequestOptions(ProductModel);

            var inventoryItemRequestOptions = new InventoryItemRequestOptions { Stock = InventoryItemModel.Stock };

            try
            {
                await _productService.UpdateAsync(productId, productRequestOptions);


                return RedirectToPage("Index");

            }
            catch(MicroStoreClientException ex)
            {
                if(ex.Erorr != null)
                {
                    if (ex.Erorr.Title != null) ModelState.AddModelError("", ex.Erorr.Title);
                    if (ex.Erorr.Type != null) ModelState.AddModelError("", ex.Erorr.Type);
                    if (ex.Erorr.Detail != null) ModelState.AddModelError("", ex.Erorr.Detail);
                    if (ex.Erorr.Errors != null)
                    {
                        ex.Erorr.Errors.ForEach(error => ModelState.AddModelError("", string.Format("{0}:{1}", error.Key, error.Value.JoinAsString(" , "))));
                    }

                    await PrepareProductModel(productId);


                    await BuildViewModel();

                    return Page();
                }

                throw ex;
            }
        }


        public async Task PrepareProductModel(string productId)
        {
            var product = await _productService.GetAsync(productId);

            ProductModel = new ProductModel
            {
                Name = product.Name,
                Sku = product.Sku,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                CategoriesId = product.ProductCategories.Select(x => x.CategoryId).ToArray(),
                Price = product.Price,
                OldPrice = product.OldPrice,
                IsFeatured = product.IsFeatured,
                

            };

            if (product.Weight != null)
            {
                ProductModel.Weight = new WeightModel
                {
                    Value = product.Weight.Value,
                    Unit = product.Weight.Unit
                };
            }

            if(product.Dimensions != null)
            {
                ProductModel.Dimension = new DimensionModel
                {
                    Lenght = product.Dimensions.Lenght,
                    Height = product.Dimensions.Height,
                    Width = product.Dimensions.Width,
                    Unit = product.Dimensions.Unit
                };
            }


        }

       
        private async Task BuildViewModel()
        {
            var categories = await _categoryService.ListAsync(new SortingRequestOptions { Desc = true });

            if (categories != null)
            {
                CategorySelectItems = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id}).ToList();
            }
        }
        private ProductRequestOptions BuildProductRequestOptions(ProductModel productModel)
        {
            var productUpdateOptions = new ProductRequestOptions
            {
                Name = productModel.Name,
                Sku = productModel.Sku,
                ShortDescription = productModel.ShortDescription,
                LongDescription = productModel.LongDescription,
                Price = productModel.Price,
                OldPrice = productModel.OldPrice,
                CategoriesIds = productModel.CategoriesId
            };

            if(productModel.Weight != null)
            {
                productUpdateOptions.Weight = new Weight
                {
                    Value = productModel.Weight.Value,
                    Unit = productModel.Weight.Unit
                };
            }

            if(productModel.Dimension != null)
            {
                productUpdateOptions.Dimensions = new Dimension
                {
                    Lenght = productModel.Dimension.Lenght,
                    Height = productModel.Dimension.Height,
                    Width = productModel.Dimension.Width,
                    Unit = productModel.Dimension.Unit
                };
            }

            return productUpdateOptions;
        }
    }
}
