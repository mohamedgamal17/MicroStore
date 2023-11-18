using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Infrastructure;
namespace MicroStore.Client.PublicWeb.Pages
{
    [CheckProfilePageCompletedFilter]
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