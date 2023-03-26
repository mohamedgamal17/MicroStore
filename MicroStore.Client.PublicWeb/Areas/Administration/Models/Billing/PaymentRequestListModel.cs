using MicroStore.ShoppingGateway.ClinetSdk.Entities;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Billing
{
    public class PaymentRequestListModel : BasePagedListModel
    {
        public List<PaymentRequestVM> Data { get; set; }
    }
}
