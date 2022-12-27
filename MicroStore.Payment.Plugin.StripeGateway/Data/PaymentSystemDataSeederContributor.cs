using MicroStore.Payment.Domain;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Payment.Plugin.StripeGateway.Data
{
    public class PaymentSystemDataSeederContributor : IDataSeedContributor
    {
        private readonly IRepository<PaymentSystem> _paymentSystemRepository;

        public PaymentSystemDataSeederContributor(IRepository<PaymentSystem> paymentSystemRepository)
        {
            _paymentSystemRepository = paymentSystemRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            bool isExist = await _paymentSystemRepository.AnyAsync(x => x.Name == StripePaymentConst.Provider);

            if(isExist)
            {
                return;
            }

            await _paymentSystemRepository.InsertAsync(new PaymentSystem
            {
                Name = StripePaymentConst.Provider,
                DisplayName = StripePaymentConst.DisplayName,
                IsEnabled = false,
                Image = StripePaymentConst.Image,
            });
        }
    }
}
