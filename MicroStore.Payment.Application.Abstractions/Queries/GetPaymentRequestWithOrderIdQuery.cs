using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentRequestWithOrderIdQuery : IQuery
    {
        public string OrderId { get; set; }
    }
}
