namespace MicroStore.Bff.Shopping.Models.Billing
{
    public class PaymentItemModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public PaymentItemModel()
        {
            ProductId = string.Empty;
            Name = string.Empty;
            Sku = string.Empty;
            Image = string.Empty;
            Quantity = 0;
            UnitPrice = 0;
        }
    }
}
