using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
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
                "/libs/intl-tel-input/build/css/intlTelInput.min.css",
                "/css/custom-data-table.css"
            });

        }
    }
}
