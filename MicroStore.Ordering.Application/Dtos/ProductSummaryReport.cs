namespace MicroStore.Ordering.Application.Dtos
{
    public  class ProductSummaryReport
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public double Average { get; set; }
        public int Units { get; set; }
        public string Date { get; set; }
    }
}
