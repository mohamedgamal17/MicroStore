using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public interface IPaymentSystemCommandService : IApplicationService
    {
        Task<ResultV2<PaymentSystemDto>> EnablePaymentSystemAsync(string systemName, bool isEnabled, CancellationToken cancellationToken = default);
    }
}
