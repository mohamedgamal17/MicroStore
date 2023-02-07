using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Domain.Shared.Dtos;

namespace MicroStore.Payment.Application.PaymentRequests
{
    public class CreatePaymentRequestCommand : ICommand<PaymentRequestDto>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalCost { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }

    public class ProcessPaymentRequestCommand : ICommand<PaymentProcessResultDto>
    {
        public Guid PaymentId { get; set; }
        public string PaymentGatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }

    public class CompletePaymentRequestCommand : ICommand<PaymentRequestDto>
    {
        public string PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }

    public class RefundPaymentRequestCommand : ICommand<PaymentRequestDto>
    {
        public Guid PaymentId { get; set; }
    }
    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
