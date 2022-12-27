namespace MicroStore.Inventory.Domain.ProductAggregate
{
    public class ProductAggregateErrorType
    {
        public static string ProductAdjustingQuantityError => "product_adjusting_quantity_error";

        public static string ProductAllocationError => "product_allocation_error";

        public static string ProductReleasingStockError => "product_releasing_stock_error";

    }
}
