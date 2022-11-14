using MicroStore.Payment.Api.Domain.Events;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Payment.Api.Domain
{
    public class PaymentRequest : AggregateRoot<Guid>
    {
        public Guid OrderId { get;private set; }
        public string OrderNumber { get; private set; }
        public string CustomerId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus State { get;private set; }
        public string?  TransctionId { get;private set; }
        public string? PaymentGateway { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? OpenedAt { get; private set; }
        public DateTime? CapturedAt { get; private set; }
        public DateTime? FaultAt { get; private set; }
        public string? FaultReason { get; private set; }


        public PaymentRequest(Guid orderId, string orderNumber,string customerId  ,decimal amoun)
            : base (Guid.NewGuid())
        {
            OrderId = orderId;

            OrderNumber = orderNumber;

            CustomerId = customerId;

            Amount = amoun;

            State = PaymentStatus.Created;

            AddLocalEvent(new PaymentCreatedEvent
            {
                PaymentId = Id,
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                CustomerId = CustomerId
            });
        }


        public void SetPaymentOpened(string transactionId, DateTime openedAt , string paymentGateway)
        {
            if (State == PaymentStatus.Created)
            {
                State = PaymentStatus.Created;
                TransctionId = transactionId;
                OpenedAt = openedAt;
                PaymentGateway = paymentGateway;
            }
        }


        public void SetPaymentCompleted(DateTime capturedAt)
        {
            if(State == PaymentStatus.Opened)
            {
                State = PaymentStatus.Completed;

                CapturedAt = capturedAt;

                AddLocalEvent(new PaymentCompletedEvent
                {
                    PaymentId = Id,
                    OrderId = OrderId,
                    OrderNumber = OrderNumber,
                    CapturedAt = capturedAt
                });
            }
        }


        public void SetPaymentFaild(string faultReason , DateTime faultAt)
        {
            if(State == PaymentStatus.Opened)
            {
                State = PaymentStatus.Faild;
                FaultReason = faultReason;
                FaultAt  = faultAt;


                AddLocalEvent(new PaymentFaildEvent
                {
                    PaymentId = Id,
                    OrderId = OrderId,
                    OrderNumber = OrderNumber,
                    CustomerId = CustomerId
                });
            }
        }

        public void VoidPayment(DateTime faultAt)
        {
            if(State != PaymentStatus.Completed)
            {
                State = PaymentStatus.Void;
                FaultAt = faultAt;
            }

        }
    }

 
}
