using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentSystemWithNameQuery : IQuery
    {
        public string SystemName { get; set; }
    }
}
