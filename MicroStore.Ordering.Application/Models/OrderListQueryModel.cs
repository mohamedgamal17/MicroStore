using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Ordering.Application.Models
{
    public class OrderListQueryModel : PagingAndSortingQueryParams
    {
        public string? OrderNumber { get; set; }
        public string? States { get; set; }
        public DateTime? StartSubmissionDate { get; set; }
        public DateTime? EndSubmissionDate { get; set; }
    }
}
