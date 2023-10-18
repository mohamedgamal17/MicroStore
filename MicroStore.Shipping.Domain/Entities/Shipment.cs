#pragma warning disable CS8618
using MicroStore.Shipping.Domain.ValueObjects;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities.Events;
namespace MicroStore.Shipping.Domain.Entities
{
    public class Shipment : FullAuditedAggregateRoot<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public Address Address { get; set; }
        public string? ShipmentExternalId { get;  set; }
        public string? ShipmentLabelExternalId { get;  set; }
        public string? TrackingNumber { get;  set; }
        public string? SystemName { get;  set; }
        public ShipmentStatus Status { get;  set; }
        public List<ShipmentItem> Items { get; set; } = new List<ShipmentItem>();     

    
        protected Shipment()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Shipment(string orderId, string orderNumber ,string userId ,Address address)
        {
            Id = Guid.NewGuid().ToString(); 
            OrderId = orderId;
            OrderNumber = orderNumber;
            UserId = userId;
            Address = address;

            AddLocalEvent(new EntityCreatedEventData<Shipment>(this));
        }


        public void Fullfill(string systemName , string shipmentExternalId)
        {
            if(Status == ShipmentStatus.Created)
            {
                SystemName = systemName;
                ShipmentExternalId = shipmentExternalId;
                Status = ShipmentStatus.Fullfilled;
                AddLocalEvent(new EntityChangedEventData<Shipment>(this));
            }
        }


        public void BuyShipmentLabel(string labelId , string trackingNumber)
        {
            if(Status == ShipmentStatus.Fullfilled)
            {
                ShipmentLabelExternalId = labelId;
                TrackingNumber = trackingNumber;
                Status = ShipmentStatus.Shipping;
                AddLocalEvent(new EntityChangedEventData<Shipment>(this));
            }
        }


        public void Complete()
        {
            if(Status == ShipmentStatus.Shipping)
            {
                Status = ShipmentStatus.Completed;
                AddLocalEvent(new EntityChangedEventData<Shipment>(this));
            }
        }
    }
}
