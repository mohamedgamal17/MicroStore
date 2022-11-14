using MicroStore.Payment.Api.Domain.Events;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Payment.Api.Domain
{
    public class Payment : AggregateRoot<Guid>
    {
        public Guid OrderId { get;private set; }
        public string OrderNumber { get; private set; }
        public string CustomerId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public PaymentStatus State { get;private set; }
        public string?  TransctionId { get;private set; }
        public string? PaymentGateway { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CapturedAt { get; set; }
        public DateTime? FaultAt { get; set; }
        public string? FaultReason { get; set; }


        public Payment(Guid orderId, string orderNumber,string customerId  ,decimal totalPrice)
            : base (Guid.NewGuid())
        {
            OrderId = orderId;

            OrderNumber = orderNumber;

            CustomerId = customerId;

            TotalPrice = totalPrice;

            State = PaymentStatus.Created;

            AddLocalEvent(new PaymentCreatedEvent
            {
                PaymentId = Id,
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                CustomerId = CustomerId
            });
        }


        public void SetPaymentOpened(string transactionId , string paymentGateway)
        {
            if (State == PaymentStatus.Created)
            {
                State = PaymentStatus.Created;
                TransctionId = transactionId;
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

    public enum PaymentStatus
    {
        Created ,

        Opened,

        Completed,

        Faild,

        Void
    }
}
