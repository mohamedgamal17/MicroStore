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
    }
    public class CreatePaymentModel : PaymentModel
    {
        public string UserId { get; set; }
    }
}
