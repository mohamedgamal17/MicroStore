namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductSalesChartDataModel
    {
        public float Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Date { get; set; }
        public bool IsForecasted { get; set; }
    }
}
