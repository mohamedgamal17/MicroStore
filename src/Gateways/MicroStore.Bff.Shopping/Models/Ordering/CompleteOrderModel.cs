namespace MicroStore.Bff.Shopping.Models.Ordering
{
    public class CompleteOrderModel
    {
        public DateTime ShippedAt { get; set; }

        public CompleteOrderModel()
        {
            ShippedAt = DateTime.MinValue;   
        }
    }
}
