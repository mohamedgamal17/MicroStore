﻿namespace MicroStore.Payment.IntegrationEvents
{
    public class PaymentCreatedIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string PaymentId { get; set; }
        public string CustomerId { get; set; }
    }
}