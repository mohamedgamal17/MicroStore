namespace MicroStore.Gateway.Shopping.Security
{
    public static class OrderingScope
    {

        public static List<string> List()
        {
            return new List<string>
            {
                Order.Read,
                Order.Write,
            };
        }

        public static class Order
        {

            public const string Read = "ordering.order.read";

            public const string Write = "ordering.order.write";

        }
    }
}
