namespace MicroStore.Ordering.Application.Abstractions.Security
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
                Order.Fullfill,
                Order.Complete,
                Order.Cancel
            };
        }

        public static class Order
        {
            public const string List = "ordering.order.list";

            public const string Read = "ordering.order.read";

            public const string Submit = "ordering.order.submit";

            public const string Fullfill = "ordering.order.fullfill";

            public const string Complete = "ordering.order.complete";

            public const string Cancel = "ordering.order.cancel";
        }
    }
}
