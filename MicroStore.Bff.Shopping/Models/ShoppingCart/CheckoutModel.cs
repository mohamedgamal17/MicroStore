namespace MicroStore.Bff.Shopping.Models.ShoppingCart
{
    public class CheckoutModel
    {
        public string ShippingAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string PaymentMethod { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
