namespace MicroStore.Bff.Shopping.Models.Billing
{
    public class ProcessPaymentModel
    {
        public string GatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
