namespace MicroStore.Inventory.Domain.Security
{
    public static class InventoryScope
    {
        public static List<string> List()
        {
            return new List<string>()
            {
                Order.Read,
            };
        }

        public static class Order
        {

            public const string Read = "inventory.order.read";

        }
    }
}
