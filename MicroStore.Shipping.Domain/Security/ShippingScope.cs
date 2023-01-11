namespace MicroStore.Shipping.Domain.Security
{
    public static class ShippingScope
    {
        public static List<string> List()
        {
            return new List<string>
            {
                Shipment.List,
                Shipment.Read,
                Shipment.Create,
                Shipment.Fullfill,
                Label.Buy,
                Rate.Retrive,
                Rate.Estimate,
                Settings.Read,
                Settings.Update,
                System.List,
                System.Read,
                System.Update
            };
        }
        public static class Shipment
        {
            public const string List = "shipping.shipment.list";

            public const string Read = "shipping.shipment.read";

            public const string Create = "shipping.shipment.create";

            public const string Fullfill = "shipping.shipment.fullfill";
        }

        public static class Label
        {
            public const string Buy = "shipping.label.buy";
        }

        public static class Rate
        {
            public const string Retrive = "shipping.rate.retrive";

            public const string Estimate = "shipping.rate.estimate";
        }

        public static class Settings
        {
            public const string Read = "shipping.settings.read";

            public const string Update = "shipping.settings.update";
        }

        public static class System
        {
            public const string List = "shipping.system.list";

            public const string Read = "shipping.system.read";

            public const string Update = "shipping.system.update";
        }   

        public static class Address
        {
            public const string Validate = "shipping.address.validate";
        }
    }
}
