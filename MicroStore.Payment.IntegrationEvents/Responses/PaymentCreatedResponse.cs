namespace MicroStore.Payment.IntegrationEvents.Responses
{
    public class PaymentCreatedResponse
    {
        public string TransactionId { get; set; }
        public string Gateway { get; set; }
    }
}
