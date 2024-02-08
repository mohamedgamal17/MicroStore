namespace MicroStore.Bff.Shopping.Models.Billing
{
    public class CompletePaymentModel
    {
        public string GatewayName { get; set; }
        public string SessionId { get; set; }

        public CompletePaymentModel()
        {
            GatewayName = string.Empty;
            SessionId = string.Empty;
        }
    }
}
