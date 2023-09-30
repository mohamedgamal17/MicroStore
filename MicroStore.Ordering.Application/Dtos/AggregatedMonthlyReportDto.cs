namespace MicroStore.Ordering.Application.Dtos
{
    public class AggregatedMonthlyReportDto
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Sum { get; set; }
        public double Average { get; set; }
        public List<MonthlyReportDto> Reports { get; set; }

    }


    public class MonthlyReportDto
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
