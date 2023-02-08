namespace MicroStore.Inventory.Domain.Security
{
    public static class InventoryScope
    {
        public static List<string> List()
        {
            return new List<string>()
            {
                Order.List,
                Order.Read,
            };
        }

        public static class Order
        {
            public const string List = "inventory.order.list";

            public const string Read = "inventory.order.read";

        }
    }
}
