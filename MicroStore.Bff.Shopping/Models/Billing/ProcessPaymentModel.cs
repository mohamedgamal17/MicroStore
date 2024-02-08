namespace MicroStore.Bff.Shopping.Models.Billing
{
    public class ProcessPaymentModel
    {
        public string GatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }

        public ProcessPaymentModel()
        {
            GatewayName = string.Empty;
            ReturnUrl = string.Empty;
            CancelUrl = string.Empty;
        }
    }
}
