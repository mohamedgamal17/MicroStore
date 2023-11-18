using MicroStore.Client.PublicWeb.Infrastructure;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
namespace MicroStore.Client.PublicWeb.Pages.Cart
{
    [CheckProfilePageCompletedFilter]
    public class IndexModel : AbpPageModel
    {
        public void OnGet()
        {
        }
    }
}
