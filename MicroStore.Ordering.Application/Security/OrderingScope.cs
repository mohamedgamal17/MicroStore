namespace MicroStore.Ordering.Application.Security
{
    public static class OrderingScope
    {

        public static List<string> List()
        {
            return new List<string>
            {
                Order.List,
                Order.Read,
                Order.Submit,
            };
        }

        public static class Order
        {
            public const string List = "ordering.order.list";

            public const string Submit = "ordering.order.submit";

            public const string Read = "ordering.order.read";
        }
    }
}
