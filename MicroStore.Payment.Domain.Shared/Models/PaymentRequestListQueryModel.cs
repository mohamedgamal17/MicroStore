using MicroStore.BuildingBlocks.Paging.Params;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Domain.Shared.Models
{
    public class PaymentRequestListQueryModel : PagingAndSortingQueryParams
    {
        public string? States { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
