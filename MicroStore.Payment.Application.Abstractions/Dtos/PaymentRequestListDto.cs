using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Dtos
{
    public class PaymentRequestListDto : CreationAuditedEntityDto<Guid>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public double Amount { get; set; }
        public string TransctionId { get; set; }
        public string PaymentGateway { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? CapturedAt { get; set; }
    }
}
