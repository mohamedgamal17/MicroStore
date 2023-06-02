using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MicroStore.Client.PublicWeb.Components.Cart
{
    public class CartWidgetStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/Shared/Components/CartWidget/cart-widget.css");
        }
    }
}
