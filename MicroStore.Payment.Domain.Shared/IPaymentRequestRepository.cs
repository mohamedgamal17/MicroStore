using MicroStore.Payment.Domain.Shared.Domain;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentRequestRepository : IRepository<PaymentRequest,Guid>
    {
        Task<PaymentRequest?> GetPaymentRequest(Guid paymentId, CancellationToken cancellationToken = default);

    }
}
