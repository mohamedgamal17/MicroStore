namespace MicroStore.ShoppingCart.Api.Models
{
    public class CheckoutModel
    {
        public Guid BillingAddressId { get; set; }
        public Guid ShippingAddressId { get; set; }
    }

}
