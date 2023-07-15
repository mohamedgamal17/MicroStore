using Volo.Abp.UI.Navigation;

namespace MicroStore.Client.PublicWeb.Menus
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
                 new ApplicationMenuItem("MicroStore.Catalog", "Catalog", icon: "nav-icon fas fa-book", requiredPermissionName: null)
                       .AddItem(new ApplicationMenuItem("MicroStore.Catalog.Product", "Manage Products", icon: "far fa-dot-circle nav-icon", url: "/Administration/Product"))
                       .AddItem(new ApplicationMenuItem("MicroStore.Catalog.Category", "Manage Categories", icon: "far fa-dot-circle nav-icon", url: "/Administration/Category"))
                       .AddItem(new ApplicationMenuItem("MicroStore.Catalog.Manufacturer", "Manage Manufacturers", icon: "far fa-dot-circle nav-icon", url: "/Administration/Manufacturer"))
              ).AddItem(
                     new ApplicationMenuItem("MicroStore.Inventory", "Inventory", icon: "nav-icon fas fa-book", requiredPermissionName: null)
                           .AddItem(new ApplicationMenuItem("MicroStore.Inventory.InventoryItem", "Manage Products", icon: "far fa-dot-circle nav-icon", url: "/Administration/inventoryitem"))
                           .AddItem(new ApplicationMenuItem("MicroStore.Inventory.Order", "Manage Orders", icon: "far fa-dot-circle nav-icon", url: "/Administration/product"))
              ).AddItem(
                     new ApplicationMenuItem("MicroStore.Sales", "Sales", icon: "nav-icon fas fa-book", requiredPermissionName: null)
                           .AddItem(new ApplicationMenuItem("MicroStore.Sales.Order", "Manage Orders", icon: "far fa-dot-circle nav-icon", url: "/Administration/order"))
                           .AddItem(new ApplicationMenuItem("MicroStore.Sales.Shipment", "Manage Shipments", icon: "far fa-dot-circle nav-icon", url: "/Administration/shipment"))
                           .AddItem(new ApplicationMenuItem("MicroStore.Sales.PaymentRequest", "Manage Payments", icon: "far fa-dot-circle nav-icon", url: "/Administration/paymentRequest"))
              );

            return Task.CompletedTask;
        }
    }
}
