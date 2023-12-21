namespace MicroStore.BuildingBlocks.Paging
{
    public class PagedResult
    {
        public int Lenght { get;  set; }
        public int Skip { get; set; }
        public long TotalCount { get;  set; }
    }

    public class PagedResult<T> : PagedResult 
    { 
        public IEnumerable<T> Items { get; set; }

        public PagedResult()
        {
            
        }
        public PagedResult(IEnumerable<T> items, long count, int skip, int lenght)
        {
            Items = items;
            TotalCount = count;
            Skip = skip;
            Lenght = lenght;
     
        }

    }
}