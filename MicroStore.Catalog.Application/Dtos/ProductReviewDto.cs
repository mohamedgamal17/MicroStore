using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Dtos
{
    public class ProductReviewDto : EntityDto<string>
    {
        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string ReplayText { get; set; }

        public string ProductId { get; set; }

        public string UserId { get; set; }
 
    }
}
