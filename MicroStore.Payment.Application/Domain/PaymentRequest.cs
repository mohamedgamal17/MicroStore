#pragma warning disable CS8618

using MicroStore.Payment.Domain.Shared.Events;
using Volo.Abp.Domain.Entities.Auditing;
namespace MicroStore.Payment.Application.Domain
{
    public class PaymentRequest : CreationAuditedAggregateRoot<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public double SubTotal { get; set; }
        public double TaxCost { get; set; }
        public double ShippingCost { get; set; }
        public double TotalCost { get; set; }
        public string? Description { get; set; }
        public List<PaymentRequestProduct> Items { get; set; }
        public string? PaymentGateway { get; private set; }
        public string? TransctionId { get; private set; }
        public PaymentStatus State { get; private set; }
        public DateTime? CapturedAt { get; private set; }
        public DateTime? RefundedAt { get; set; }
        public DateTime? FaultAt { get; private set; }


        public PaymentRequest()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void Complete(string paymentGateway, string transactionId, DateTime capturedAt)
        {
            if (State == PaymentStatus.Waiting)
            {
                PaymentGateway = paymentGateway;
                TransctionId = transactionId;
                CapturedAt = capturedAt;
                State = PaymentStatus.Payed;

                AddLocalEvent(new PaymentAcceptedEvent
                {
                    OrderId = OrderId.ToString(),
                    OrderNumber = OrderNumber,
                    PaymentId = Id,
                    PaymentGateway = paymentGateway,
                    TransactionId = transactionId
                });

            }
        }


        public void MarkAsPaid(DateTime capturedAt)
        {
            if (State == PaymentStatus.UnPayed)
            {
                CapturedAt = capturedAt;
                State = PaymentStatus.Payed;
            }
        }

        public void MarkAsUnPaid(string paymentGateway)
        {
            if (State == PaymentStatus.Waiting)
            {
                PaymentGateway = paymentGateway;

                AddLocalEvent(new PaymentAcceptedEvent
                {
                    OrderId = OrderId.ToString(),
                    OrderNumber = OrderNumber,
                    PaymentId = Id,
                    PaymentGateway = paymentGateway
                });

                State = PaymentStatus.UnPayed;
            }
        }

        public void MarkAsFaild(string paymentGateway, DateTime faultAt)
        {
            if (State == PaymentStatus.Waiting)
            {
                PaymentGateway = paymentGateway;
                FaultAt = faultAt;
                State = PaymentStatus.Faild;
            }
        }

        public void MarkAsRefunded(DateTime refundedAt, string? description)
        {
            if (State == PaymentStatus.Payed)
            {
                RefundedAt = refundedAt;
                Description = description;
                State = PaymentStatus.Refunded;
            }
        }
    }
}
