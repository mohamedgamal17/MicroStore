namespace MicroStore.Ordering.Application.Dtos
{
    public class OrderSummaryReport
    {
        public int TotalUnPayed { get; set; }
        public int TotalUnApproved { get; set; }
        public int TotalUnfullfilled { get; set; }
        public int TotalUnShipped { get; set; }
        public int TotalCompleted { get; set; }
        public int TotalCancelled { get; set; }
    }
}
