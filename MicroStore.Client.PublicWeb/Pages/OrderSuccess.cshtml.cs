using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Infrastructure;

namespace MicroStore.Client.PublicWeb.Pages
{
    [CheckProfilePageCompletedFilter]
    public class OrderSuccessModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
