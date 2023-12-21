using MicroStore.ShoppingGateway.ClinetSdk.Entities;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering
{
    public class OrderListModel : BasePagedListModel
    {
        public List<OrderAggregateVM> Data { get; set; }
    }
}
