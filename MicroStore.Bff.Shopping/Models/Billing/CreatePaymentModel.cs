namespace MicroStore.Bff.Shopping.Models.Billing
{
    public class PaymentModel
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalCost { get; set; }
        public string Description { get; set; }
        public List<PaymentItemModel> Items { get; set; }

        public PaymentModel()
        {
            OrderId = string.Empty;
            OrderNumber = string.Empty;
            ShippingCost = 0;
            TaxCost = 0;
            SubTotal = 0;
            TotalCost = 0;
            Description = string.Empty;
            Items = new List<PaymentItemModel>();
        }
    }
    public class CreatePaymentModel : PaymentModel
    {
        public string UserId { get; set; }

        public CreatePaymentModel()
        {
            UserId = string.Empty;
        }
    }
}
