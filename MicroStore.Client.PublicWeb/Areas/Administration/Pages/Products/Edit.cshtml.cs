using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Products
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ProductModel ProductModel { get; set; } = new ProductModel();
        public void OnGet()
        {
        }
    }
}
