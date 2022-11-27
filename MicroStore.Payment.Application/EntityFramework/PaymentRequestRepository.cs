using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Domain;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Payment.Application.EntityFramework
{
    public class PaymentRequestRepository : EfCoreRepository<PaymentDbContext, PaymentRequest, Guid>, IPaymentRequestRepository
    {
        public PaymentRequestRepository(IDbContextProvider<PaymentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<PaymentRequest?> GetPaymentRequest(Guid paymentId,CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == paymentId,cancellationToken);
        }
    }
}
