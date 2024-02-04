namespace MicroStore.Bff.Shopping.Models.Billing
{
    public class CompletePaymentModel
    {
        public string GatewayName { get; set; }
        public string SessionId { get; set; }
    }
}
