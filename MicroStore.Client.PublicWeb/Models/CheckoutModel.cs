namespace MicroStore.Client.PublicWeb.Models
{
    public class CheckoutModel
    {
        public AddressModel ShippingAddress { get; set; }
        public bool UseAnotherBillingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public string PaymentMethod { get; set; }
    }
}
