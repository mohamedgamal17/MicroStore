﻿#nullable disable
using MicroStore;

namespace MicroStore.Payment.Application.Domain.Events
{
    public class PaymentFaildEvent
    {
        public Guid PaymentId { get; init; }
        public Guid OrderId { get; init; }
        public string OrderNumber { get; init; }
        public string CustomerId { get; set; }

        public DateTime FaultDate { get; set; }
    }
}