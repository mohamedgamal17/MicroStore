using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentRequestWithOrderIdQuery : IQuery<PaymentRequestDto>
    {
        public string OrderId { get; set; }
    }
}
