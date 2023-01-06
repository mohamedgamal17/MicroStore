using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentSystemQuery : IQuery
    {
        public Guid SystemId { get; set; }
    }
}
