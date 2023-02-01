﻿namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory
{
    public class OrderItem : BaseEntity<Guid>
    {
        public string ExternalItemId { get; set; }
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}