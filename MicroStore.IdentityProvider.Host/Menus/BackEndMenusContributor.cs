using Volo.Abp.UI.Navigation;

namespace MicroStore.IdentityProvider.Host.Menus
{
    public class BackEndMenusContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == ApplicationMenusDefaults.BackEnd)
            {
                await ConfigureBackEndMenuAsync(context);
            }

        }

        private Task ConfigureBackEndMenuAsync(MenuConfigurationContext context)
        {
            context.Menu.AddItem(
                 new ApplicationMenuItem("MicroStore.Identity", "Identity", icon: "nav-icon fas fa-book", requiredPermissionName: null)
                       .AddItem(new ApplicationMenuItem("MicroStore.Identity.User", "Manage Users", icon: "far fa-dot-circle nav-icon", url: "/BackEnd/User"))
                       .AddItem(new ApplicationMenuItem("MicroStore.Identity.Role", "Manage Roles", icon: "far fa-dot-circle nav-icon", url: "/BackEnd/Role"))
              ).AddItem(
                     new ApplicationMenuItem("MicroStore.IdentityServer", "Identity Server", icon: "nav-icon fas fa-book", requiredPermissionName: null)
                           .AddItem(new ApplicationMenuItem("MicroStore.IdentityServer.Client", "Manage Clients", icon: "far fa-dot-circle nav-icon", url: "/BackEnd/Client"))
                           .AddItem(new ApplicationMenuItem("MicroStore.IdentityServer.ApiResource", "Manage Api Resource", icon: "far fa-dot-circle nav-icon", url: "/BackEnd/ApiResource"))
                           .AddItem(new ApplicationMenuItem("MicroStore.IdentityServer.ApiScope", "Manage Api ApiScope", icon: "far fa-dot-circle nav-icon", url: "/BackEnd/ApiScope"))
              );

            return Task.CompletedTask;
        }
    }
}
