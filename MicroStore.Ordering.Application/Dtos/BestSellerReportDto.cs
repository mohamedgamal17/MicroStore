using Volo.Abp.Application.Dtos;
namespace MicroStore.Ordering.Application.Dtos
{
    public class BestSellerReportDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string? Thumbnail { get; set; }
        public double Amount { get; set; }
        public double Quantity { get; set; }
    }
}
