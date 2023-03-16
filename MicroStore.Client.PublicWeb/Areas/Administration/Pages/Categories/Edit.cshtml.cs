#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using NUglify.Helpers;
using System.Net;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly CategoryService _categoryService;


        [BindProperty]
        public CategoryModel CategoryModel { get; set; } = new CategoryModel();

        public EditModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> OnGet(string categoryId)
        {
           
            CategoryModel = await PrepareCategoryModel(categoryId);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string categoryId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }      

            try
            {
                var categoryUpdateOptions = new CategoryRequestOptions
                {
                    Name = CategoryModel.CategoryName,
                    Description = CategoryModel.Description
                };

                await _categoryService.UpdateAsync(categoryId,categoryUpdateOptions);

                return RedirectToPage("Index");
            }
            catch(MicroStoreClientException ex)
            {


                if(ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    if (ex.Erorr.Title != null) ModelState.AddModelError("", ex.Erorr.Title);
                    if (ex.Erorr.Type != null) ModelState.AddModelError("", ex.Erorr.Type);
                    if (ex.Erorr.Detail != null) ModelState.AddModelError("", ex.Erorr.Detail);
                    if (ex.Erorr.Errors != null)
                    {
                        ex.Erorr.Errors.ForEach(error => ModelState.AddModelError("", string.Format("{0}:{1}", error.Key, error.Value)));
                    }
                    return Page();
                }

                throw ex;
            }
        }

        private async Task<CategoryModel> PrepareCategoryModel(string categoryId)
        {
            var category = await _categoryService.GetAsync(categoryId);
            return new CategoryModel
            {
                CategoryName = category.Name,
                Description = category.Description
            };
        }
    }
}
