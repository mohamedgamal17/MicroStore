using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages
{
    [Authorize]
    public class ForbiddenModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
