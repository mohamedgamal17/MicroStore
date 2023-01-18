using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentSystemQuery : IQuery<PaymentSystemDto>
    {
        public Guid SystemId { get; set; }
    }
}
