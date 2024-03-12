using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MicroStore.Client.PublicWeb.Bundling
{
    public class BackEndThemeStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]{
                "/backend/css/main.css",
            });

        }
    }
}
