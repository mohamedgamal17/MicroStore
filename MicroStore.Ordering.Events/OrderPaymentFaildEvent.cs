namespace MicroStore.Ordering.Events
{
    public class OrderPaymentFaildEvent // will modifiy name later
    {
        public Guid OrderId { get; set; }
        public string FaultReason { get; set; }  
        public DateTime FaultDate { get; set; }

    }
}
