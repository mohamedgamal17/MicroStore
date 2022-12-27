﻿using MicroStore.Shipping.Domain.ValueObjects;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Shipping.Domain.Entities
{
    public class Shipment :BasicAggregateRoot<Guid>
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public Address Address { get; set; }
        public string? ShipmentExternalId { get;  set; }
        public string? ShipmentLabelExternalId { get;  set; }
        public string? TrackingNumber { get;  set; }
        public string? SystemName { get;  set; }
        public ShipmentStatus Status { get;  set; }
        public List<ShipmentItem> Items { get; set; } = new List<ShipmentItem>();     
        private Shipment() { }  
        public Shipment(string orderId,string userId ,Address address)
        {
            OrderId = orderId;
            UserId = userId;
            Address = address;
        }


        public void Fullfill(string systemName , string shipmentExternalId)
        {
            if(Status == ShipmentStatus.Created)
            {
                SystemName = systemName;
                ShipmentExternalId = shipmentExternalId;
                Status = ShipmentStatus.Fullfilled;
            }
        }


        public void BuyShipmentLabel(string labelId , string trackingNumber)
        {
            if(Status == ShipmentStatus.Fullfilled)
            {
                ShipmentLabelExternalId = labelId;
                TrackingNumber = trackingNumber;
                Status = ShipmentStatus.Shipping;
            }
        }


        public void Complete()
        {
            if(Status == ShipmentStatus.Shipping)
            {
                Status = ShipmentStatus.Completed;
            }
        }
    }
}