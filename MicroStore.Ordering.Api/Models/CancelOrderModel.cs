namespace MicroStore.Ordering.Api.Models
{
    public class CancelOrderModel
    {
        public string Reason { get; set; }

        public DateTime CancellationDate { get; set; }
    }
}
