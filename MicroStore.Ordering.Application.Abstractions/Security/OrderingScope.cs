namespace MicroStore.Ordering.Application.Abstractions.Security
{
    public static class OrderingScope
    {

        public static class Order
        {
            public static readonly string List = "ordering.order.list";

            public static readonly string Read = "ordering.order.read";

            public static readonly string Submit = "ordering.order.submit";

            public static readonly string Fullfill = "ordering.order.fullfill";

            public static readonly string Complete = "ordering.order.complete";

            public static readonly string Cancel = "ordering.order.cancel";
        }
    }
}
