using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentRequestQuery : IQuery
    {
        public Guid PaymentRequestId { get; set; }

    }
}
