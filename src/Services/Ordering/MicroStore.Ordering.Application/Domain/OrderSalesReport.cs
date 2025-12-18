using System;
using System.Collections.Generic;

namespace MicroStore.Ordering.Application.Domain
{
    public class OrderSalesReport
    {
        public string CurrentState { get; set; }
        public long TotalOrders { get; set; }
        public double TotalShippingPrice { get; set; }
        public double TotalTaxPrice { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
