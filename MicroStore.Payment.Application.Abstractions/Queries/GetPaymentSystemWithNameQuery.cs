using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentSystemWithNameQuery : IQuery<PaymentSystemDto>
    {
        public string SystemName { get; set; }
    }
}
