using Volo.Abp.Application.Dtos;
namespace MicroStore.Ordering.Application.Domain
{
    public class BestSellerReport 
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }

    }
}
