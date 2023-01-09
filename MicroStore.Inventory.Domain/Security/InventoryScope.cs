namespace MicroStore.Inventory.Domain.Security
{
    public static class InventoryScope
    {

        public static class Product
        {
            public static readonly string List = "inventory.product.list";

            public static readonly string Read = "inventory.product.read";

            public static readonly string AdjustQuantity = "inventory.product.adjustquantity";
        }

        public static class Order
        {
            public static readonly string List = "inventory.order.list";

            public static readonly string Read = "inventory.order.read";

        }
    }
}
