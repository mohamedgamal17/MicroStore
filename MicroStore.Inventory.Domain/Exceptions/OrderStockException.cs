namespace MicroStore.Inventory.Domain.Exceptions
{
    public class OrderStockException : Exception
    {
        public OrderStockException(Dictionary<string,string> details)
        {
            Details = details;
        }
        public Dictionary<string, string> Details { get; set; }
    }
}
