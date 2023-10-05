using System;
using System.Collections.Generic;
namespace MicroStore.Ordering.Application.Dtos
{
    public class OrderSummaryReport
    {
        public int TotalOrders { get; set; }
        public double SumShippingTotalCost { get; set; }
        public double SumTaxTotalCost { get; set; }
        public double SumSubTotalCost { get; set; }
        public double SumTotalCost { get; set; }
        public string Date { get; set; }
    }
}
