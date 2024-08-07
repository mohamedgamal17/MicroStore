﻿namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering
{
    public class OrderSalesChartDataModel
    {
        public long TotalOrders { get; set; }
        public double SumShippingTotalCost { get; set; }
        public double SumTaxTotalCost { get; set; }
        public double SumTotalCost { get; set; }
        public string Date { get; set; }
        public bool IsForecasted { get; set; }
    }
}
