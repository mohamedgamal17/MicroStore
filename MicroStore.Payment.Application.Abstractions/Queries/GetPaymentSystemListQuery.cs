using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Queries
{
    public class GetPaymentSystemListQuery : IQuery<ListResultDto<PaymentSystemDto>>
    {
    }
}
