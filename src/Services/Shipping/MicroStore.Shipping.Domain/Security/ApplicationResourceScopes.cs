namespace MicroStore.Shipping.Domain.Security
{
    public static class ApplicationResourceScopes
    {

        public const string Access = "shipping.access";
        public static class Shipment
        {
            public const string Read = "shipping.read";
        }
     
    }
}
