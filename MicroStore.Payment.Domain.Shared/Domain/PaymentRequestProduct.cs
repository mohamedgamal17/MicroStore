using Volo.Abp.Domain.Entities;

namespace MicroStore.Payment.Domain.Shared.Domain
{
    public class PaymentRequestProduct : Entity<Guid>
    {

        public string ProductId { get; set; }
        public string Name { get; set; }
        public string? Sku { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


        public PaymentRequestProduct()
        {

        }

    }
}
