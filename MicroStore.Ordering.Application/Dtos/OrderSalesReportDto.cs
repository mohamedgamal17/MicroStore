namespace MicroStore.Ordering.Application.Dtos
{
    public class OrderSalesReportDto
    {
        public long TotalOrders { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
