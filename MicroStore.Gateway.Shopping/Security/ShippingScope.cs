namespace MicroStore.Gateway.Shopping.Security
{
    public static class ShippingScope
    {
        public static List<string> List()
        {
            return new List<string>
            {
                Shipment.Read,
                Shipment.Write,
            };
        }
        public static class Shipment
        {

            public const string Read = "shipping.shipment.read";

            public const string Write = "shipping.shipment.write";

        }
    }
}
