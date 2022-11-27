#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Dtos;
namespace MicroStore.Payment.Application.Commands.Requests
{
    public class CreatePaymentRequestCommand: ICommand<PaymentRequestCreatedDto>
    {
        public string OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string  UserId { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal SubtTotal { get; set; }
        public decimal TotalCost { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }


    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
