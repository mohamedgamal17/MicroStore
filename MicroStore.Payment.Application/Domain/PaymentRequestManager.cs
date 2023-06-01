using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
namespace MicroStore.Payment.Domain
{
    public class PaymentRequestManager : IPaymentRequestManager, ITransientDependency , IUnitOfWorkEnabled
    {
        private IRepository<PaymentRequest> _paymentRequestRepository;

        private readonly IObjectMapper _objectMapper;

        public PaymentRequestManager(IRepository<PaymentRequest> paymentRequestRepository, IObjectMapper objectMapper)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _objectMapper = objectMapper;
        }

        public async Task<PaymentRequestDto> Complete(string paymentId, string paymentGateway, string transactionId, DateTime capturedAt , CancellationToken cancellationToken = default)
        {
            var payment = await RetrivePaymentRequest(paymentId, cancellationToken);


            payment.Complete(paymentGateway,transactionId,capturedAt);

            await _paymentRequestRepository.UpdateAsync(payment);


            return _objectMapper.Map<PaymentRequest, PaymentRequestDto>(payment);
        }

        public async Task<PaymentRequestDto> GetPaymentRequest(string paymentId, CancellationToken cancellationToken = default)
        {
            var payment = await RetrivePaymentRequest(paymentId, cancellationToken);

            return _objectMapper.Map<PaymentRequest,PaymentRequestDto>(payment);
        }

      
        public async Task<PaymentRequestDto> Refund(string paymentId, DateTime refundedAt, string? description = null , CancellationToken cancellationToken = default)
        {
            var payment = await RetrivePaymentRequest(paymentId, cancellationToken);

            payment.MarkAsRefunded(refundedAt, description);

            await _paymentRequestRepository.UpdateAsync(payment);

            return _objectMapper.Map<PaymentRequest, PaymentRequestDto>(payment);
        }



        private async Task<PaymentRequest> RetrivePaymentRequest(string paymentId, CancellationToken cancellationToken = default)
        {
            var query = await _paymentRequestRepository.GetQueryableAsync();    

           return await query.Include(x=> x.Items).SingleAsync(x => x.Id == paymentId, cancellationToken);
        }

        public async Task<PaymentRequestDto> MarkAsFaild(string paymentId,string paymentGateway, DateTime faultAt, CancellationToken cancellationToken = default)
        {
            var payment = await RetrivePaymentRequest(paymentId, cancellationToken);

            payment.MarkAsFaild(paymentGateway, faultAt);

            await _paymentRequestRepository.UpdateAsync(payment);

            return _objectMapper.Map<PaymentRequest, PaymentRequestDto>(payment);
        }
    }
}
