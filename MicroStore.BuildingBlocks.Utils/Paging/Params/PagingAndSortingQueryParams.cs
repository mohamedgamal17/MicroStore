namespace MicroStore.BuildingBlocks.Utils.Paging.Params
{
    public class PagingAndSortingQueryParams : PagingQueryParams
    {
        public string SortBy { get; set; } = string.Empty;

        public bool Desc { get; set; }

    }
}
