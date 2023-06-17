using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages
{
    public class HomeModel : PageModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 24;
        public IActionResult OnGet(int currentPage = 1)
        {
            CurrentPage = currentPage;

            return Page();
        }
    }
}
