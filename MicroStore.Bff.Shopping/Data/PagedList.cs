namespace MicroStore.Bff.Shopping.Data
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Skip { get; set; }
        public int Length { get; set; }
        public long TotalCount { get; set; }
    }
}
