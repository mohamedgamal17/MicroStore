﻿namespace MicroStore.Payment.IntegrationEvents
{

    [Obsolete("Use create payment integration event insted")]
    public class CreatePaymentRequest
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
