using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.SignalR;
using Volo.Abp.Modularity;

namespace MicroStore.Client.PublicWeb.Components.Basket
{
    public class BasketWidgetStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/Shared/Components/BasketWidget/basket-widget.css");
        }
    }

    [DependsOn(typeof(SignalRBrowserScriptContributor))]
    public class BasketWidgetScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/Shared/Components/BasketWidget/basket-widget.js");

        }

    }
}
