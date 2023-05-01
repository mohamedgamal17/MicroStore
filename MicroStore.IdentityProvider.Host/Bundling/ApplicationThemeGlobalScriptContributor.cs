using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.Host.Bundling
{

    public class ApplicationThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/summernote/summernote.min.js",
                "/libs/admin-lte/js/adminlte.min.js",
                "/libs/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js",
                "/js/abp.config.js"
            });
        }
    }
}
