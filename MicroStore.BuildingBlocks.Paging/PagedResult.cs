namespace MicroStore.BuildingBlocks.Paging
{
    public class PagedResult
    {
        public int Lenght { get; protected set; }
        public int Skip { get; set; }
        public int TotalCount { get; protected set; }
    }

    public class PagedResult<T> : PagedResult 
    { 
        public IEnumerable<T> Items { get; set; }

        public PagedResult(IEnumerable<T> items, int count, int skip, int lenght)
        {
            Items = items;
            TotalCount = count;
            Skip = skip;
            Lenght = lenght;
     
        }

    }
}