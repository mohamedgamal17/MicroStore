using System;
using System.Collections.Generic;
namespace MicroStore.Inventory.Application.Abstractions.Dtos
{
    public class OrderListDto
    {
        public Guid OrderId { get; set; }
        public string ExternalOrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string ExternalPaymentId { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public AddressDto BillingAddres { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public bool IsCancelled { get; set; }
    }
}
