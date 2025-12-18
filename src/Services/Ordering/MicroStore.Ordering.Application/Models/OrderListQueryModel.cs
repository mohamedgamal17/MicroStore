using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Ordering.Application.Models
{
    public class OrderListQueryModel : PagingAndSortingQueryParams
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string States { get; set; } = string.Empty;
        public DateTime StartSubmissionDate { get; set; } = DateTime.MinValue;
        public DateTime EndSubmissionDate { get; set; } = DateTime.MinValue;
    }
}
