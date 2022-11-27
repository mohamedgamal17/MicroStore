﻿#nullable disable
namespace MicroStore.Payment.Application.Dtos
{
    public class PaymentRequestProductDto
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
