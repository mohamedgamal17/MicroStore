namespace MicroStore.BuildingBlocks.Paging
{
    public class PagedResult
    {
        public int PageNumber { get; protected set; }
        public int PageSize { get; protected set; }
        public int TotalPages { get; protected set; }
        public int TotalCount { get; protected set; }


        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class PagedResult<T> : PagedResult 
    { 
        public IEnumerable<T> Items { get; set; }

        public PagedResult(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

    }
}