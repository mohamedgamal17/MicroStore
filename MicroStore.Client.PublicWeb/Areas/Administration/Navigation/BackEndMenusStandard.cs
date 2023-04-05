namespace MicroStore.Client.PublicWeb.Areas.Administration.Navigation
{
    public static class BackEndMenusStandard
    {
        public static class ProductMenus
        {
            public const string Index = "MicroStore.Catalog.Product";

            public const string Create = "MicroStore.Catalog.Product.Create";

            public const string Edit = "MicroStore.Catalog.Product.Edit";
        }

        public static class CategoryMenus
        {
            public const string Index = "MicroStore.Catalog.Category";

            public const string Create = "MicroStore.Catalog.Category.Create";

            public const string Edit = "MicroStore.Catalog.Category.Edit";
        }

        public static class OrderMenus
        {
            public const string Index = "MicroStore.Sales.Order";

            public const string View = "MicroStore.Sales.Order.View";
        }


        public static class PaymentRequestMenus
        {
            public const string Index = "MicroStore.Sales.PaymentRequest";

            public const string View = "MicroStore.Sales.PaymentRequest.View";
        }

        public static class ShipmentMenus
        {
            public const string Index = "MicroStore.Sales.Shipment";

            public const string View = "MicroStore.Sales.Shipment.View";

            public const string Fullfill = "MicroStore.Sales.Shipment.Fullfill";

            public const string Label = "MicroStore.Sales.Shipment.Label";
        }
    }
}
