using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.SignalR;
using Volo.Abp.Modularity;

namespace MicroStore.Client.PublicWeb.Components.CheckoutInformation
{
    [DependsOn(typeof(SignalRBrowserScriptContributor))]
    public class CheckoutInformationWidgetScriptContrinutor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/Shared/Components/CheckoutInformationWidget/checkout-information-widget.js");

        }
    }
}
