using MicroStore.BuildingBlocks.AspNetCore.Attributes;

namespace MicroStore.BuildingBlocks.AspNetCore.Models
{
    public class PagingParamsQueryString
    {
        [CustomFromQuery(nameof(Skip))]
        public int Skip { get; set; } = 0;

        [CustomFromQuery(nameof(Lenght))]
        public int Lenght { get; set; } = 10;
    }
}
