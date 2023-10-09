namespace MicroStore.AspNetCore.UI.Models
{
    public class HtmlChartDataModel
    {
        public List<ChartDataSetModel> Data { get; set; }
    }


    public class ChartDataSetModel
    {
        public string X { get; set; }
        public double Y { get; set; }

    }
}
