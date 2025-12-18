namespace MicroStore.Bff.Shopping.Infrastructure
{
    public class BffAggregatorScopes
    {
        public static List<string> List()
        {
            return new List<string>
            {
                Catalog.Access,
                Basket.Access,
                Ordering.Access,
                Ordering.Read,
                Ordering.Write,
                Billing.Access,
                Billing.Read,
                Billing.Write,
                Shipping.Access,
                Shipping.Read,
                Inventory.Access,
                Geographic.Access,
                Profiling.Access,
                Profiling.Read,
               Profiling.Write
            };
        }

        public static class Catalog
        {
            public const string Access = "catalog.access";
        }

        public static class Basket
        {
            public const string Access = "basket.access";
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
        }

        public static class Geographic
        {
            public const string Access = "geographic.access";
        }

        public static class Profiling
        {
            public const string Access = "profiling.access";
            public const string Read = "profiling.read";
            public const string Write = "profiling.write";
        }
    }

}
