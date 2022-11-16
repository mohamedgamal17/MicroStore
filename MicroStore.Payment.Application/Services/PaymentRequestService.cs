using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace MicroStore.Payment.Application.Services
{
    public class PaymentRequestService : IPaymentRequestService , IUnitOfWorkEnabled , ITransientDependency
    {
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;

        public PaymentRequestService(IRepository<PaymentRequest> paymentRequestRepository)
        {
            _paymentRequestRepository = paymentRequestRepository;
        }

        public async Task<Result<PaymentCompletedDto>> CompletePayment(string transactionId, string paymentGatewayapi, DateTime capturedDate)
        {
            PaymentRequest? paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.TransctionId == transactionId 
                && x.PaymentGateway == paymentGatewayapi);

            if(paymentRequest == null)
            {
                return Result.Failure<PaymentCompletedDto>("Payment request is not exist");
            }


            Result result =  paymentRequest.CanCompletePayment();

            if (result.IsFailure)
            {
                return Result.Failure<PaymentCompletedDto>(result.Error);
            }

            paymentRequest.SetPaymentCompleted(capturedDate);


            await _paymentRequestRepository.UpdateAsync(paymentRequest);

            return Result.Success(new PaymentCompletedDto
            {
                PaymentId = paymentRequest.Id,
                OrderId = paymentRequest.OrderId,
                OrderNumber = paymentRequest.OrderNumber,
                CustomerId = paymentRequest.CustomerId,
                Amount = paymentRequest.Amount,
                TransctionId = paymentRequest.TransctionId,
                PaymentGateway = paymentRequest.PaymentGateway,
                CreatedAt = paymentRequest.CreatedAt,
                OpenedAt = paymentRequest.OpenedAt,
                CapturedAt = paymentRequest.CapturedAt
            });

        }

        public async Task<Result<PaymentDto>> GetPayment(Guid paymentId)
        {
            PaymentRequest? paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId);

            if(paymentRequest == null)
            {
                return Result.Failure<PaymentDto>("Payment request is not exist");
            }

            return Result.Success(new PaymentDto
            {
                PaymentId = paymentRequest.Id,
                OrderId = paymentRequest.OrderId,
                OrderNumber = paymentRequest.OrderNumber,
                CustomerId = paymentRequest.CustomerId,
                Amount = paymentRequest.Amount,
                TransctionId = paymentRequest.TransctionId,
                PaymentGateway = paymentRequest.PaymentGateway,
                CreatedAt = paymentRequest.CreatedAt,
                OpenedAt = paymentRequest.OpenedAt,
                CapturedAt = paymentRequest.CapturedAt

            });
        }

        public async Task<Result<PaymentFaildDto>> SetPaymentFaild(Guid paymentId, string faultReason, DateTime faultDate)
        {
            PaymentRequest? paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId);

            if (paymentRequest == null)
            {
                return Result.Failure<PaymentFaildDto>("Payment request is not exist");
            }

            Result result = paymentRequest.CanSetPaymentFaild();

            if (result.IsFailure)
            {
                return Result.Failure<PaymentFaildDto>(result.Error);
            }


            paymentRequest.SetPaymentFaild(faultReason, faultDate);

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

            return Result.Success(new PaymentFaildDto
            {
                PaymentId = paymentRequest.Id,
                OrderId = paymentRequest.OrderId,
                OrderNumber = paymentRequest.OrderNumber,
                CustomerId = paymentRequest.CustomerId,
                Amount = paymentRequest.Amount,
                TransctionId = paymentRequest.TransctionId,
                PaymentGateway = paymentRequest.PaymentGateway,
                CreatedAt = paymentRequest.CreatedAt,
                OpenedAt = paymentRequest.OpenedAt,
                FaultDate = faultDate
            });
        }

        public async Task<Result<PaymentStartedDto>> StartPayment(Guid paymentId, string transactionId, string paymentGatewayapi, DateTime openedAt)
        {
            PaymentRequest? paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId);

            if (paymentRequest == null)
            {
                return Result.Failure<PaymentStartedDto>("Payment request is not exist");
            }

            Result result = paymentRequest.CanStartPayment();

            if (result.IsFailure)
            {
                return Result.Failure<PaymentStartedDto>(result.Error);
            }

            paymentRequest.SetPaymentOpened(transactionId, openedAt, paymentGatewayapi);

            await _paymentRequestRepository.UpdateAsync(paymentRequest);

            return Result.Success(new PaymentStartedDto
            {
                PaymentId = paymentRequest.Id,
                OrderId = paymentRequest.OrderId,
                OrderNumber = paymentRequest.OrderNumber,
                CustomerId = paymentRequest.CustomerId,
                Amount = paymentRequest.Amount,
                TransctionId = paymentRequest.TransctionId,
                PaymentGateway = paymentRequest.PaymentGateway,
                CreatedAt = paymentRequest.CreatedAt,
                OpenedAt = paymentRequest.OpenedAt,
            });
        }
    }
}
