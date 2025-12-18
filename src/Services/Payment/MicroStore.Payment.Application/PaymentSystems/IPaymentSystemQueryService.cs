using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public interface IPaymentSystemQueryService : IApplicationService
    {
        Task<Result<List<PaymentSystemDto>>> ListAsync(CancellationToken cancellationToken = default);

    }
}
