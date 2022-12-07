#nullable disable
using MicroStore.Payment.Application.Commands.Requests;
namespace MicroStore.Payment.Api.Models
{
    public class CreatePaymentRequestModel
    {
        public string OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string UserId { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal SubtTotal { get; set; }
        public decimal TotalCost { get; set; }
        public List<OrderItemModel> Items { get; set; }

    }
}
