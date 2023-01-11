namespace MicroStore.Inventory.Domain.Security
{
    public static class InventoryScope
    {
        public static List<string> List()
        {
            return new List<string>()
            {
                Product.List ,
                Product.Read,
                Product.AdjustQuantity,
                Order.List,
                Order.Read,
            };
        }
        public static class Product
        {
            public const string List = "inventory.product.list";

            public const string Read = "inventory.product.read";

            public const string AdjustQuantity = "inventory.product.adjustquantity";
        }

        public static class Order
        {
            public const string List = "inventory.order.list";

            public const string Read = "inventory.order.read";

        }
    }
}
