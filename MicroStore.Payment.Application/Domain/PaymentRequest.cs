using Ardalis.GuardClauses;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Domain.Events;
using MicroStore.Payment.Application.Extensions;
using MicroStore.Payment.Domain.Shared.Events;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Payment.Application.Domain
{
    public class PaymentRequest : BasicAggregateRoot<Guid>
    {
        public Guid OrderId { get; private set; }
        public string OrderNumber { get; private set; }
        public string CustomerId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus State { get; private set; }
        public string? TransctionId { get; private set; }
        public string? PaymentGateway { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? OpenedAt { get; private set; }
        public DateTime? CapturedAt { get; private set; }
        public DateTime? FaultAt { get; private set; }
        public string? FaultReason { get; private set; }

        private PaymentRequest()
        {

        }
        public PaymentRequest(Guid orderId, string orderNumber, string customerId, decimal amount)
            : base(Guid.NewGuid())
        {
            OrderId = orderId;

            OrderNumber = orderNumber;

            CustomerId = customerId;

            Amount = amount;

            State = PaymentStatus.Created;

            AddLocalEvent(new PaymentCreatedEvent
            {
                PaymentId = Id,
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                CustomerId = CustomerId
            });
        }


        public void SetPaymentOpened(string transactionId, DateTime openedAt, string paymentGateway)
        {
            Guard.Against.InvalidResult(CanStartPayment());

            State = PaymentStatus.Opened;
            TransctionId = transactionId;
            OpenedAt = openedAt;
            PaymentGateway = paymentGateway;

        }


        public Result CanStartPayment()
        {
            if(State == PaymentStatus.Created)
            {
                return Result.Success();
            }

            return Result.Failure($"Can not start payment in state : {State}." +
                    "Payment must be in opened state or compeleted state");
        }

        public void SetPaymentCompleted(DateTime capturedAt)
        {

            Guard.Against.InvalidResult(CanCompletePayment());

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


        public Result CanCompletePayment()
        {
            if(State == PaymentStatus.Opened)
            {
                return Result.Success();
            }

            return Result.Failure($"Can not complete payment in state : {State}." +
                     "Payment must be in opened state or compeleted state");
        }


        public void SetPaymentFaild(string faultReason, DateTime faultAt)
        {

            Guard.Against.InvalidResult(CanSetPaymentFaild());

            State = PaymentStatus.Faild;

            FaultReason = faultReason;

            FaultAt = faultAt;

            AddLocalEvent(new PaymentFaildEvent
            {
                PaymentId = Id,
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                CustomerId = CustomerId,
                FaultDate = faultAt
            });
        }


        public Result CanSetPaymentFaild()
        {

            if(State == PaymentStatus.Opened)
            {
                return Result.Success();
            }

            return Result.Failure($"Can not void payment in state : {State}." +
                    "Payment must be in opened state or compeleted state");

        }

        public void VoidPayment(DateTime faultAt)
        {

            State = PaymentStatus.Void;

            FaultAt = faultAt;

            AddLocalEvent(new PaymentVoidedEvent
            {
                PaymentId = Id,
                TransactionId = TransctionId,
                CustomerId = CustomerId,
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                FaultDate = faultAt,
                PaymentGatewayApi = PaymentGateway
            });

        }
    }


}
