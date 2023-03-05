#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Categories
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public CategoryModel CategoryModel { get; set; } = new CategoryModel();
        public void OnGet()
        {
        }
    }
}
