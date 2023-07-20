using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MicroStore.Client.PublicWeb.Bundling
{
    public class FrontEndThemeStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]{
                "/libs/animate.css/animate.min.css",
                "/libs/ion-rangeslider/css/ion.rangeSlider.min.css",
                "/frontend/css/vendors/font-awesome.css",
                "/frontend/css/vendors/themify-icons.css",
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
                "/libs/ion-rangeslider/js/ion.rangeSlider.min.js",
                "/frontend/js/script.js",
                "/frontend/js/main.js"
            });

        }
    }
}
