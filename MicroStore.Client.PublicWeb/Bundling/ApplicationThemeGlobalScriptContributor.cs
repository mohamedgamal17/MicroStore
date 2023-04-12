using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Modularity;

namespace MicroStore.Client.PublicWeb.Bundling
{

    public class ApplicationThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/summernote/summernote.min.js",
                "/lib/admin-lte/js/adminlte.min.js"
            });
        }
    }
}
