namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models
{
    public  class ListModel
    {
        public string Draw { get; set; }
    }

    public  class PagedListModel : ListModel
    {
        public PagedListModel()
        {
            Length = 10;
        }

        public int PageNumber => Skip / PageSize + 1;
        public int Skip => Start;
        public int PageSize => Length;
        public int Length { get; set; }
        public int Start { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered => RecordsTotal;
    }
}
