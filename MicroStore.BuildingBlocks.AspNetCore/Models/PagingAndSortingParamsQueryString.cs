using MicroStore.BuildingBlocks.AspNetCore.Attributes;

namespace MicroStore.BuildingBlocks.AspNetCore.Models
{
    public class PagingAndSortingParamsQueryString  : PagingParamsQueryString
    {
        [CustomFromQuery(nameof(SortBy))]
        public string? SortBy { get; set; }

        [CustomFromQuery(nameof(Desc))]
        public bool Desc { get; set; }
    }
}
