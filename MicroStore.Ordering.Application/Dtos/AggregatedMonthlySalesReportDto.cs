namespace MicroStore.Ordering.Application.Dtos
{
    public class AggregatedMonthlySalesReportDto
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Sum { get; set; }
        public double Average { get; set; }
        public List<MonthlySalesReportDto> Reports { get; set; }

    }


    public class MonthlySalesReportDto
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public int Count { get; set; }
        public double Sum { get; set; }
        public double Average { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }


 
}
