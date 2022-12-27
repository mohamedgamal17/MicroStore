using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Domain;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Payment.PluginInMemoryTest.EntityFramework
{
    public class PaymentRequestRepository : EfCoreRepository<PaymentInMemoryDbContext, PaymentRequest, Guid>, IPaymentRequestRepository , ITransientDependency
    {

        public PaymentRequestRepository(IDbContextProvider<PaymentInMemoryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<PaymentRequest?> GetPaymentRequest(Guid paymentId, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);
        }


    }
}
