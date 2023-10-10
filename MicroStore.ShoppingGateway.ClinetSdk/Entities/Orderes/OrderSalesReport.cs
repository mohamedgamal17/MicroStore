﻿
namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes
{
    public class OrderSalesReport
    {
        public long TotalOrders { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
