﻿namespace MicroStore.Ordering.Application.Abstractions.Dtos
{
    public class OrderListDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public AddressDto BillingAddress { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ShipmentId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string CurrentState { get; set; }
    }
}
