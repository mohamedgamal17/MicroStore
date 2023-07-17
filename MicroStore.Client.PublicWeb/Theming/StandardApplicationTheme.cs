using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Client.PublicWeb.Theming
{

    public class StandardApplicationLayout 
    {
        public const string BackEnd = "MicroStoreBackendLayout";

        public const string FrontEnd = "MicroStoreFrontEndLayout";
    }

    [ThemeName(Name)]
    public class StandardApplicationTheme : ITheme , ITransientDependency
    {
        public const string Name = "StandardTheme";
        public string GetLayout(string name, bool fallbackToDefault = true)
        {
            return name switch
            {
                StandardApplicationLayout.BackEnd => "~/Areas/Administration/Views/Shared/_Layout.cshtml",
                StandardApplicationLayout.FrontEnd => "~/Pages/Shared/_MultiKartLayout.cshtml",
                _ => fallbackToDefault ? "~/Areas/Administration/Views/Shared/_MultiKartLayout.cshtml" : string.Empty
            };
        }
    }
}
