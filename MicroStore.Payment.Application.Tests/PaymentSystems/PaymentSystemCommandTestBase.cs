using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Application.Domain;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Tests.PaymentSystems
{
    public abstract class PaymentSystemCommandTestBase : BaseTestFixture
    {
        public Task<PaymentSystem> GenerateFakePaymentSystem()
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<PaymentSystem>>();

                return repository.InsertAsync(new PaymentSystem
                {
                    Name = Guid.NewGuid().ToString(),
                    DisplayName = Guid.NewGuid().ToString(),
                    IsEnabled = false,
                    Image = Guid.NewGuid().ToString(),
                });
            });
        }
    }
}
