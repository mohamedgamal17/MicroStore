using MicroStore.ShoppingGateway.ClinetSdk.Entities;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments
{
    public class ShipmentListModel : BasePagedListModel
    {
        public List<ShipmentVM> Data { get; set; }
    }
}
