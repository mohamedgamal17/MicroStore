﻿namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing
{
    public class PaymentRequestList : CreationAuditedEntity<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public double Amount { get; set; }
        public string TransctionId { get; set; }
        public string PaymentGateway { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? CapturedAt { get; set; }
    }
}
