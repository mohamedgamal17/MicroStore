namespace MicroStore.Ordering.Application.Domain
{
    public class ProductSalesReport
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
