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


            };
        }
        public static class Shipment
        {
            public const string List = "shipping.shipment.list";

            public const string Read = "shipping.shipment.read";
        }

      
    }
}
