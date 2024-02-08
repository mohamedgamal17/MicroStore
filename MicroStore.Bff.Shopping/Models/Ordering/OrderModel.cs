﻿using MicroStore.Bff.Shopping.Models.Common;

namespace MicroStore.Bff.Shopping.Models.Ordering
{
    public class OrderModel
    {
        public string UserId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
    }
}
