using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Infrastructure;
namespace MicroStore.Client.PublicWeb.Pages.Orders
{
    [Authorize]
    [CheckProfilePageCompletedFilter]
    public class DetailsModel : PageModel
    {
        public Guid OrderId { get; set; }
        public void OnGet(Guid id)
        {
            OrderId = id;
        }
    }
}
