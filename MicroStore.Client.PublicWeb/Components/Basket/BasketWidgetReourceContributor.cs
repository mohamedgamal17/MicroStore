using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
namespace MicroStore.Client.PublicWeb.Components.Basket
{
    public class BasketWidgetStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/Shared/Components/BasketWidget/basket-widget.css");
        }
    }

    public class BasketWidgetScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/Shared/Components/BasketWidget/basket-widget.js");

        }

    }
}
