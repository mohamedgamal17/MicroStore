using MicroStore.Payment.Domain;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Abstractions
{
    public interface IPaymentRequestRepository : IRepository<PaymentRequest, Guid>
    {
        Task<PaymentRequest?> GetPaymentRequest(Guid paymentId, CancellationToken cancellationToken = default);

    }
}
