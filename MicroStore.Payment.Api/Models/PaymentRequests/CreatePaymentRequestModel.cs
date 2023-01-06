#nullable disable
using MicroStore;
using MicroStore.Payment.Application.Abstractions.Commands;

namespace MicroStore.Payment.Api.Models.PaymentRequests
{
    public class CreatePaymentRequestModel
    {
        public string OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string UserId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubtTotal { get; set; }
        public double TotalCost { get; set; }
        public List<OrderItemModel> Items { get; set; }

    }
}
