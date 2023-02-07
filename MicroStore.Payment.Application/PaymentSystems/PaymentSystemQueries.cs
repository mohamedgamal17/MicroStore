using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public class GetPaymentSystemListQuery : IQuery<ListResultDto<PaymentSystemDto>>
    {
    }
    public class GetPaymentSystemQuery : IQuery<PaymentSystemDto>
    {
        public Guid SystemId { get; set; }
    }

    public class GetPaymentSystemWithNameQuery : IQuery<PaymentSystemDto>
    {
        public string SystemName { get; set; }
    }
}
