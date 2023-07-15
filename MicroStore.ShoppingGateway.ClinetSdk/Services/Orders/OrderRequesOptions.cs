﻿using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderSubmitRequestOptions
    {
    
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItemRequestOptions> Items { get; set; }
    }

    public class OrderCreateRequestOptions : OrderSubmitRequestOptions
    {
        public string UserId { get; set; }
    }

    public class OrderFullfillRequestOptions
    {
        public string ShipmentId { get; set; }
    }

    public class OrderCancelRequestOptions
    {
        public string Reason { get; set; }

    }
    public class OrderItemRequestOptions
    {
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderListRequestOptions : PagingAndSortingRequestOptions
    {
        public string OrderNumber { get; set; }
        public string States { get; set; }
        public DateTime? StartSubmissionDate { get; set; }
        public DateTime? EndSubmissionDate { get; set; }
    }
}
