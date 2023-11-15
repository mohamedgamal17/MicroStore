using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Infrastructure;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages
{
    [CheckProfilePageCompletedFilter]
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
