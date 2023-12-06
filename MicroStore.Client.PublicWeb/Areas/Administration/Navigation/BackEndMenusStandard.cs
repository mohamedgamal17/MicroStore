namespace MicroStore.Client.PublicWeb.Areas.Administration.Navigation
{
    public static class BackEndMenusStandard
    {
        public static class Dasboard
        {
           public const string Index = "MicroStore.Dashboard";
        }
        public static class Product
        {
            public const string Index = "MicroStore.Catalog.Product";

            public const string Create = "MicroStore.Catalog.Product.Create";

            public const string Edit = "MicroStore.Catalog.Product.Edit";

            public const string Report = "MicroStore.Catalog.Product.Report";

            public const string Forecast = "MicroStore.Catalog.Product.Forecast";
        }

        public static class Category
        {
            public const string Index = "MicroStore.Catalog.Category";

            public const string Create = "MicroStore.Catalog.Category.Create";

            public const string Edit = "MicroStore.Catalog.Category.Edit";
        }

        public static class Manufacturer
        {
            public const string Index = "MicroStore.Catalog.Manufacturer";

            public const string Create = "MicroStore.Catalog.Manufacturer.Create";

            public const string Edit = "MicroStore.Catalog.Manufacturer.Edit";
        }

        public static class Order
        {
            public const string Index = "MicroStore.Sales.Order";

            public const string View = "MicroStore.Sales.Order.View";
        }


        public static class PaymentRequest
        {
            public const string Index = "MicroStore.Sales.PaymentRequest";

            public const string View = "MicroStore.Sales.PaymentRequest.View";
        }

        public static class Shipment

        {
            public const string Index = "MicroStore.Sales.Shipment";

            public const string View = "MicroStore.Sales.Shipment.View";

            public const string Fullfill = "MicroStore.Sales.Shipment.Fullfill";

            public const string Label = "MicroStore.Sales.Shipment.Label";
        }

        public static class Inventory
        {
            public class InventoryItem
            {
                public const string Index = "MicroStore.Inventory.InventoryItem";

                public const string Create = "MicroStore.Inventory.InventoryItem.Create";

                public const string Edit = "MicroStore.Inventory.InventoryItem.Edit";
            }


        }

        public class Report
        {
            public const string OrderSales = "MicroStore.Report.OrderSales";

            public const string CountriesSales = "MicroStore.Report.CountriesSales";

            public const string SalesForecasting = "MicroStore.Report.OrderSales.Forecasting";
        }

        public class Country
        {
            public const string Index = "MicroStore.Configuation.Country";

            public const string Create = "MicroStore.Configuation.Country.Create";

            public const string Edit = "MicroStore.Configuation.Country.Edit";

            public const string Forecasting = "MicroStore.Configuation.Country.Forecasting";

            public const string Report = "MicroStore.Configuation.Country.Report";
        }

        public class Settings
        {
            public const string Shipping = "MicroStore.Configuation.Shipping";
        }
    }
}
