#nullable disable
using Volo.Abp.Application.Dtos;
namespace MicroStore.Payment.Domain.Shared.Dtos
{
    public class PaymentRequestDto : FullAuditedEntityDto<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public double SubTotal { get; set; }
        public double TaxCost { get; set; }
        public double ShippingCost { get; set; }
        public double TotalCost { get; set; }
        public string? Description { get; set; }
        public List<PaymentRequestProductDto> Items { get; set; }
        public string PaymentGateway { get; private set; }
        public string TransctionId { get; private set; }
        public string Status { get; private set; }
        public DateTime CapturedAt { get; private set; }
        public DateTime RefundedAt { get; set; }
        public DateTime FaultAt { get; private set; }
    }
}
