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
        public DateTime? CapturedAt { get; set; }
        public string? FaultReason { get; set; }


        public Payment(Guid orderId, string orderNumber,string customerId  ,decimal totalPrice)
            : base (Guid.NewGuid())
        {
            OrderId = orderId;

            OrderNumber = orderNumber;

            CustomerId = customerId;

            TotalPrice = totalPrice;

            State = PaymentStatus.Created;
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
            }
        }


        public void SetPaymentFaild(string faultReason)
        {
            if(State == PaymentStatus.Opened)
            {
                State = PaymentStatus.Faild;
                FaultReason = faultReason;
            }
        }

        public void VoidPayment()
        {
            if(State != PaymentStatus.Completed)
            {
                State = PaymentStatus.Void;
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
