#nullable disable
using MicroStore;
using MicroStore.Payment.Application.PaymentRequests;

namespace MicroStore.Payment.Api.Models.PaymentRequests
{

    public class PaymentRequestModel
    {
        public string OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubtTotal { get; set; }
        public double TotalCost { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }


    public class CreatePaymentRequestModel : PaymentRequestModel
    {
        public string UserId { get; set; }

    }
}
