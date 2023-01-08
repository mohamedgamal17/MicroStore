﻿namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class ShipmentListDto
    {
        public Guid ShipmentId { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public AddressDto Address { get; set; }
        public string ShipmentExternalId { get; set; }
        public string TrackingNumber { get; set; }
        public string ShipmentLabelExternalId { get; set; }
        public string SystemName { get; set; }
        public string Status { get; set; }
    }
}