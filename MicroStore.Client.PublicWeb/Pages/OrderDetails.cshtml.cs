using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages
{
    public class OrderDetailsModel : PageModel
    {
        public  Guid  OrderId { get; set; }

        public void OnGet(Guid id)
        {
            OrderId = id;
        }
    }
}
