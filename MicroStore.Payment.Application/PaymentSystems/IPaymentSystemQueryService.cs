using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public interface IPaymentSystemQueryService : IApplicationService
    {
        Task<UnitResult<PaymentSystemDto>> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<UnitResult<PaymentSystemDto>> GetBySystemNameAsync(string name, CancellationToken cancellationToken = default);
        Task<UnitResult<List<PaymentSystemDto>>> ListPaymentSystemAsync(CancellationToken cancellationToken = default);

        
    }
}
