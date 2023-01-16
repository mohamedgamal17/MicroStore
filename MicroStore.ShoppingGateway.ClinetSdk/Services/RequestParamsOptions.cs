
namespace MicroStore.ShoppingGateway.ClinetSdk.Services
{
    [Serializable]
    public class EmptyRequst
    {
        public static EmptyRequst Empty = new EmptyRequst();
        private EmptyRequst() { }
    }

    public interface IQueryRequestOptions
    {
    }


    [Serializable]
    public class SortingRequestOptions : IQueryRequestOptions
    {
        public string SortBy { get; set; }
        public bool Desc { get; set; }
    }

    [Serializable]
    public class PagingReqeustOptions : IQueryRequestOptions
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


    [Serializable]
    public class PagingAndSortingRequestOptions : PagingReqeustOptions
    {
        public string SortBy { get; set; }
        public bool Desc { get; set; }
    }
}
