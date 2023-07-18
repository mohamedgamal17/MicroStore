using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MicroStore.Client.PublicWeb.Bundling
{
    public class FrontEndThemeStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]{
                "/libs/animate.css/animate.min.css",
                "/frontend/css/themify-icons.css",
                "/frontend/css/style.css"
            });

        }
    }

    public class FronEndThemeScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]{
                "/frontend/js/menu.js",
                "/libs/lazysizes/lazysizes.min.js",
                "/frontend/js/script.js",
                "/frontend/js/main.js"
            });

        }
    }
}
