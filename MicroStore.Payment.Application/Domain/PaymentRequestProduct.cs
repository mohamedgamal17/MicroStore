using Volo.Abp.Domain.Entities;

namespace MicroStore.Payment.Domain
{
    public class PaymentRequestProduct : Entity<Guid>
    {

        public string ProductId { get; set; }
        public string Name { get; set; }
        public string? Sku { get; set; }
        public string? Thumbnail { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }


        public PaymentRequestProduct()
        {

        }

    }
}
