namespace MicroStore.Ordering.Application.Dtos
{
    public class ProductSalesReportDto
    {
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
