using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Modularity;

namespace MicroStore.Client.PublicWeb.Bundling
{
    public class ApplicationThemeGlobalStyleContributor : BundleContributor
    {

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[] 
            {
                "/libs/admin-lte/css/adminlte.min.css",
                "/libs/summernote/summernote.min.css",
            });
        }
    }
}
