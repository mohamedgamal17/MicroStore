using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Infrastructure;
namespace MicroStore.Client.PublicWeb.Pages
{
    [CheckProfilePageCompletedFilter]
    public class OrderDetailsModel : PageModel
    {
        public  Guid  OrderId { get; set; }

        public void OnGet(Guid id)
        {
            OrderId = id;
        }
    }
}
