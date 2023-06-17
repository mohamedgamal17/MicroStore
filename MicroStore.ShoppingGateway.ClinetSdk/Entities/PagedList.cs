namespace MicroStore.ShoppingGateway.ClinetSdk.Entities
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int Skip { get; set; }
        public int Lenght { get; set; }
        public int PageNumber => (Skip / PageSize) + 1;
        public int PageSize => Lenght;
        public int TotalPages=> (int) Math.Ceiling((decimal) (Lenght / PageSize));
        public int TotalCount { get; set; }
        public bool HasPreviousPage => PageNumber != 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
