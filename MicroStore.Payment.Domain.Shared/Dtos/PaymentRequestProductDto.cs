#nullable disable
using MicroStore;

namespace MicroStore.Payment.Domain.Shared.Dtos
{
    public class PaymentRequestProductDto
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
