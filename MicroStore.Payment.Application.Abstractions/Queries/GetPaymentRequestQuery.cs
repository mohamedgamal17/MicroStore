using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentRequestQuery : IQuery<PaymentRequestDto>
    {
        public Guid PaymentRequestId { get; set; }

    }
}
