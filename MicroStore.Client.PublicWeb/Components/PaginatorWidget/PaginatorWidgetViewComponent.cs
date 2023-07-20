using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Components.PaginatorWidget
{
    [Widget(AutoInitialize = true,
        StyleFiles = new string[] { "/Pages/Shared/Components/PaginatorWidget/paginator-widget.css" }
     )]
    public class PaginatorWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke(PagerModel model)
        {
            return View(model) ;
        } 
    }

}
