using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Components.ProfileWidget
{
    [Widget(AutoInitialize = true)]
    public class ProfileWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var user = (User)HttpContext.Items[HttpContextSharedItemsConsts.UserProfile]!;

            if (user == null)
            {
                throw new InvalidOperationException("User profile cannot be null");
            }

            var model = new ProfileWidgetViewComponentModel
            {
                User = user
            };

            return View(model);
        }
    }

    public class ProfileWidgetViewComponentModel
    {
        public User User { get; set; }
    }
}
