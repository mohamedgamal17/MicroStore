using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public interface IPaymentSystemQueryService : IApplicationService
    {
        Task<ResultV2<PaymentSystemDto>> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<ResultV2<PaymentSystemDto>> GetBySystemNameAsync(string name, CancellationToken cancellationToken = default);
        Task<ResultV2<List<PaymentSystemDto>>> ListPaymentSystemAsync(CancellationToken cancellationToken = default);

        
    }
}
