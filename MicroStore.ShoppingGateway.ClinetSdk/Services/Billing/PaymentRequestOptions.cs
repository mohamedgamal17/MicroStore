namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Billing
{
  
    public class PaymentCreateRequestOptions
    {
        public string OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string UserId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubtTotal { get; set; }
        public double TotalCost { get; set; }
        public List<PaymentProductCreateRequestOptions> Items { get; set; }
    }

    public class PaymentProcessRequestOptions
    {
        public string PaymentGatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }


    public class PaymentCompleteRequestOptions
    {
        public string PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }

    public class PaymentProductCreateRequestOptions
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
