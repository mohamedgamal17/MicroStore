using MicroStore.BuildingBlocks.Paging.Params;
namespace MicroStore.Payment.Domain.Shared.Models
{
    public class PaymentRequestListQueryModel : PagingAndSortingQueryParams
    {
        public string? OrderNumber { get; set; }
        public string? States { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
