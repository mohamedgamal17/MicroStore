namespace MicroStore.Client.PublicWeb.Areas.Administration.Models
{
    public abstract class BaseListModel
    {
        public string Draw { get; set; }
    }

    public abstract class BasePagedListModel : BaseListModel
    {
        public BasePagedListModel()
        {
            Length = 10;
        }

        public int PageNumber => ( Skip / PageSize ) + 1 ;
        public int Skip => Start;
        public int PageSize => Length;
        public int Length { get; set; }
        public int Start { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered => RecordsTotal;
    }
}
