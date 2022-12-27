﻿using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Domain;
using System.Net;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Payment.Application.Commands
{
    public class RefundPaymentRequestCommandHandler : CommandHandler<RefundPaymentRequestCommand>
    {
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;

        private readonly IPaymentMethodResolver _paymentMethodResolver;
        public RefundPaymentRequestCommandHandler(IRepository<PaymentRequest> paymentRequestRepository, IPaymentMethodResolver paymentMethodResolver)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _paymentMethodResolver = paymentMethodResolver;
        }

        public override async Task<ResponseResult> Handle(RefundPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository
                .SingleOrDefaultAsync(x => x.Id == request.PaymentId);

            if(paymentRequest == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo { Message = $"Payment request with id :{request.PaymentId}, is not exist" });
            }

            if(paymentRequest.State != PaymentStatus.Payed)
            {
                var errorInfo = new ErrorInfo
                {
                    Message = $"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                 $"in  {PaymentStatus.Payed}"
                };

                return ResponseResult.Failure((int)HttpStatusCode.BadRequest, errorInfo);
            }


            var unitResult = await _paymentMethodResolver.Resolve(paymentRequest.PaymentGateway!);

            var paymentMethod = unitResult.Value;

            await paymentMethod.Refund(request.PaymentId, cancellationToken);

            return ResponseResult.Success((int) HttpStatusCode.Accepted);
        }
    }
}
