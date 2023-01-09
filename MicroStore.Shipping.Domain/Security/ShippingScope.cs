namespace MicroStore.Shipping.Domain.Security
{
    public static class ShippingScope
    {
        public static class Shipment
        {
            public static readonly string List = "shipping.shipment.list";

            public static readonly string Read = "shipping.shipment.read";

            public static readonly string Create = "shipping.shipment.create";

            public static readonly string Fullfill = "shipping.shipment.fullfill";
        }

        public static class Label
        {
            public static readonly string Buy = "shipping.shipment.buy";
        }

        public static class Rate
        {
            public static readonly string Retrive = "shipping.rate.retrive";

            public static readonly string Estimate = "shipping.rate.estimate";
        }

        public static class Settings
        {
            public static readonly string Read = "shipping.settings.read";

            public static readonly string Update = "shipping.settings.update";
        }

        public static class System
        {
            public static readonly string List = "shipping.system.list";

            public static readonly string Read = "shipping.system.read";

            public static readonly string Update = "shipping.system.update";
        }
    }
}
