namespace MicroStore.ShoppingGateway.ClinetSdk.Entities
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int Skip { get; set; }
        public int Lenght { get; set; }
        public int PageNumber => (Skip / PageSize) + 1;
        public int PageSize => Lenght;
        public int TotalCount { get;  set; }
    }
}
