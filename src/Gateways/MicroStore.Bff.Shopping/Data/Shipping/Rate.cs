using Microsoft.CodeAnalysis;
using MicroStore.Bff.Shopping.Data.Common;
using System;

namespace MicroStore.Bff.Shopping.Data.Shipping
{
    public class Rate
    {
        public string Name { get; set; }
        public Money Money { get; set; }
        public int EstimatedDays { get; set; }
        public DateTime? ShippingDate { get; set; }
    }
}
