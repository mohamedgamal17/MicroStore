﻿using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;
using MicroStore.Payment.Application.Extensions;
using System.Net;

namespace MicroStore.Payment.Application.Commands
{
    public class CompletePaymentRequestCommandHandler : CommandHandler<CompletePaymentRequestCommand>
    {

        private readonly IPaymentMethodResolver _paymentResolver;

        private readonly IPaymentRequestRepository _paymentRequestRepository;
        public CompletePaymentRequestCommandHandler(IPaymentMethodResolver paymentResolver, IPaymentRequestRepository paymentRequestRepository)
        {
            _paymentResolver = paymentResolver;
            _paymentRequestRepository = paymentRequestRepository;
        }

        public override async Task<ResponseResult> Handle(CompletePaymentRequestCommand request, CancellationToken cancellationToken)
        {
 
            var unitResult  = await _paymentResolver.Resolve(request.PaymentGatewayName);

            if (unitResult.IsFailure)
            {
                return unitResult.ConvertFaildUnitResult();
            }

            var paymentMethod = unitResult.Value;

            var result =  await paymentMethod.Complete(new CompletePaymentModel
            {
                Token = request.Token,

            },cancellationToken);


            return ResponseResult.Success((int)HttpStatusCode.Accepted);
        }
    }
}
