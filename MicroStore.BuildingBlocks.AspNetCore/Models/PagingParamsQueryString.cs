using MicroStore.BuildingBlocks.AspNetCore.Attributes;

namespace MicroStore.BuildingBlocks.AspNetCore.Models
{
    public class PagingParamsQueryString
    {
        [CustomFromQuery(nameof(PageNumber))]
        public int PageNumber { get; set; } = 1;

        [CustomFromQuery(nameof(PageSize))]
        public int PageSize { get; set; } = 10;
    }
}
