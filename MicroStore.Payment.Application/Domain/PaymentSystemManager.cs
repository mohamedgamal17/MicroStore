//using MicroStore.Payment.Application.Configuration;
//using MicroStore.Payment.Domain.Shared;
//using MicroStore.Payment.Domain.Shared.Configuration;
//using MicroStore.Payment.Domain.Shared.Dtos;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Domain.Repositories;
//using Volo.Abp.ObjectMapping;
//using Volo.Abp.Uow;

//namespace MicroStore.Payment.Application.Domain
//{
//    public class PaymentSystemManager : IPaymentSystemManager , IUnitOfWorkEnabled, ITransientDependency
//    {
//        private readonly IRepository<PaymentSystem> _paymentSystemRepository;

//        private readonly IObjectMapper _objectMapper;
//        public PaymentSystemManager(IRepository<PaymentSystem> paymentSystemRepository, IObjectMapper objectMapper)
//        {
//            _paymentSystemRepository = paymentSystemRepository;
//            _objectMapper = objectMapper;
//        }

//        public async Task<PaymentSystemDto> GetPaymentSystem(string name)
//        {
//            var paymentSystem = await _paymentSystemRepository.SingleAsync(x => x.Name == name);

//            return _objectMapper.Map<PaymentSystem, PaymentSystemDto>(paymentSystem);
//        }

//        public async Task TryToCreate(string name, string displayName, string image)
//        {

//            var paymentSystem = await _paymentSystemRepository.SingleOrDefaultAsync(x => x.Name == name);

//            if(paymentSystem != null)
//            {
//                return;
//            }


//            paymentSystem = new PaymentSystem
//            {
//                Name = name,
//                DisplayName = displayName,
//                Image = image,
//                IsEnabled = true
//            };

//            await _paymentSystemRepository.InsertAsync(paymentSystem);
//        }

//        public async Task TryToDeleate(string name, string displayName, string image)
//        {
//            var paymentSystem = await _paymentSystemRepository.SingleOrDefaultAsync(x => x.Name == name);

//            if (paymentSystem == null)
//            {
//                return;
//            }

//            await _paymentSystemRepository.DeleteAsync(paymentSystem);
//        }
//    }
//}
