using MicroStore.BuildingBlocks.AspNetCore.Attributes;
namespace MicroStore.BuildingBlocks.AspNetCore.Models
{
    public class SortingParamsQueryString 
    {
        [CustomFromQuery(nameof(SortBy))]
        public string? SortBy { get; set; }

        [CustomFromQuery(nameof(Desc))]
        public bool Desc { get; set; }
    }
}
