using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages
{
    public class CartModel : AbpPageModel
    {
        public CartModel()
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Checkout");
        }
    }
}
