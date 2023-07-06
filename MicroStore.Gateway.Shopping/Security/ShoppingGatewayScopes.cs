namespace MicroStore.Gateway.Shopping.Security
{
    public static class ShoppingGatewayScopes
    {


        public static List<string> List()
        {
            return new List<string>
            {
                Ordering.Access,
                Ordering.Read,
                Ordering.Write,
                Billing.Access,
                Billing.Read,
                Billing.Write,
                Shipping.Access,
                Shipping.Read,
                Inventory.Access,
                Inventory.Read,
                Inventory.Write
            };
        }

        public static class Ordering
        {
            public const string Access = "ordering.access";

            public const string Read = "ordering.read";

            public const string Write = "ordering.write";

        }
        public static class Billing
        {
            public const string Access = "billing.access";
            public const string Read = "billing.read";

            public const string Write = "billing.write";

        }

        public static class Shipping
        {
            public const string Access = "shipping.access";
            public const string Read = "shipping.read";


        }

        public static class Inventory
        {
            public const string Access = "inventory.access";
            public const string Read = "inventory.read";

            public const string Write = "inventory.write";

        }
    }
}
