namespace MicroStore.Bff.Shopping.Models.Ordering
{
    public class CancelOrderModel
    {
        public string Reason { get; set; }

        public CancelOrderModel()
        {
            Reason = string.Empty;
        }
    }
}
