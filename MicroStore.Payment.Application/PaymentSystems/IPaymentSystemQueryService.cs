using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public interface IPaymentSystemQueryService : IApplicationService
    {
        Task<UnitResultV2<PaymentSystemDto>> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<UnitResultV2<PaymentSystemDto>> GetBySystemNameAsync(string name, CancellationToken cancellationToken = default);
        Task<UnitResultV2<List<PaymentSystemDto>>> ListPaymentSystemAsync(CancellationToken cancellationToken = default);

        
    }
}
