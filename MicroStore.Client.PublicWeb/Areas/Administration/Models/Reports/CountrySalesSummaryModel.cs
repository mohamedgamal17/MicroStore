using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Reports
{
    public class CountrySalesSummaryModel : BasePagedListModel
    {
        [BindNever]
        public List<CountrySalesSummaryVM> Data { get; set; }
    }
}
