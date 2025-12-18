using MicroStore.Bff.Shopping.Data;

namespace MicroStore.Bff.Shopping.Data.Billing
{
    public class PaymentItem : Entity<string>
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
