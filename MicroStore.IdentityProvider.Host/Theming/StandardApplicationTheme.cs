using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.Host.Theming
{

    public class StandardApplicationLayout
    {
        public const string Administration = "MicroStoreAdministrationLayout";

        public const string Account = "MicroStoreAccountLayout";
    }

    [ThemeName(Name)]
    public class StandardApplicationTheme : ITheme, ITransientDependency
    {
        public const string Name = "StandardTheme";
        public string GetLayout(string name, bool fallbackToDefault = true)
        {
            return name switch
            {
                StandardApplicationLayout.Administration => "/Areas/BackEnd/Views/Shared/_Layout.cshtml",
                StandardApplicationLayout.Account => "/Pages/Shared/_Layout.cshtml",
                _ => fallbackToDefault ? "~/Pages/Shared/_Layout.cshtml" : string.Empty
            };
        }
    }
}
