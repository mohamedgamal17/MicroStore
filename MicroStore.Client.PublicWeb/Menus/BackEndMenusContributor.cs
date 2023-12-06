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
            context.Menu
                .AddItem(
                    new ApplicationMenuItem("MicroStore.Dashboard","Dashboard",icon: "nav-icon fas fa-desktop",
                    requiredPermissionName: null, url: "/Administration/")
                )
                .AddItem(
                 new ApplicationMenuItem("MicroStore.Catalog", "Catalog", icon: "nav-icon fas fa-book", requiredPermissionName: null)
                       .AddItem(new ApplicationMenuItem("MicroStore.Catalog.Product", "Manage Products", icon: "far fa-dot-circle nav-icon", url: "/Administration/Product"))
                       .AddItem(new ApplicationMenuItem("MicroStore.Catalog.Category", "Manage Categories", icon: "far fa-dot-circle nav-icon", url: "/Administration/Category"))
                       .AddItem(new ApplicationMenuItem("MicroStore.Catalog.Manufacturer", "Manage Manufacturers", icon: "far fa-dot-circle nav-icon", url: "/Administration/Manufacturer"))
              ).AddItem(
                     new ApplicationMenuItem("MicroStore.Inventory", "Inventory", icon: "nav-icon fas fa-warehouse", requiredPermissionName: null)
                           .AddItem(new ApplicationMenuItem("MicroStore.Inventory.InventoryItem", "Manage Products", icon: "far fa-dot-circle nav-icon", url: "/Administration/inventoryitem"))
              ).AddItem(
                     new ApplicationMenuItem("MicroStore.Sales", "Sales", icon: "nav-icon fas fa-shopping-cart", requiredPermissionName: null)
                           .AddItem(new ApplicationMenuItem("MicroStore.Sales.Order", "Manage Orders", icon: "far fa-dot-circle nav-icon", url: "/Administration/order"))
                           .AddItem(new ApplicationMenuItem("MicroStore.Sales.Shipment", "Manage Shipments", icon: "far fa-dot-circle nav-icon", url: "/Administration/shipment"))
                           .AddItem(new ApplicationMenuItem("MicroStore.Sales.PaymentRequest", "Manage Payments", icon: "far fa-dot-circle nav-icon", url: "/Administration/paymentRequest"))
              ).AddItem(
                     new ApplicationMenuItem("MicroStore.Report", "Reports", icon: "nav-icon fas fa-chart-line", requiredPermissionName: null)
                           .AddItem(new ApplicationMenuItem("MicroStore.Report.OrderSales", "Sales", icon: "far fa-dot-circle nav-icon", url: "/Administration/Reports/Sales"))
                           .AddItem(new ApplicationMenuItem("MicroStore.Report.CountriesSales", "Countries Sales", icon: "far fa-dot-circle nav-icon", url: "/Administration/Reports/CountriesSales"))
              )
              .AddItem(
                     new ApplicationMenuItem("MicroStore.Configuation", "Configuration", icon: "nav-icon fas fa-cogs", requiredPermissionName: null)
                        .AddItem(new ApplicationMenuItem("MicroStore.Configuation.Country", "Countries", icon: "far fa-dot-circle nav-icon ", url: "/Administration/Country")
                        )
                        .AddItem(new ApplicationMenuItem("MicroStore.Configuation.Shipping", "Configure Shipping", icon: "far fa-dot-circle nav-icon ", url: "/Administration/Settings/Shipping")
                        )
                );

            return Task.CompletedTask;
        }
    }
}
