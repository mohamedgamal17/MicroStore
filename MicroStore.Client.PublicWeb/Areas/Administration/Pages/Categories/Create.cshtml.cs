#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly CategoryService _categoryService;


        [BindProperty]
        public CategoryModel CategoryModel { get; set; } = new CategoryModel();


        public CreateModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoryCreateOptions = new CategoryRequestOptions
            {
                Name = CategoryModel.CategoryName,
                Description = CategoryModel.Description
            };

            try
            {
                await _categoryService.CreateAsync(categoryCreateOptions);

                return RedirectToPage("Index");
            }
            catch(MicroStoreClientException ex)
            {

                if(ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("", ex.Erorr.Title);
                    ModelState.AddModelError("", ex.Erorr.Detail);
                    ModelState.AddModelError("", ex.Erorr.Type);
                }

                throw ex;
            }
        }
    }
}
