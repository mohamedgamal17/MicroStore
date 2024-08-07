﻿using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;

namespace MicroStore.Payment.Domain.Shared
{
    public interface IPaymentMethodProvider
    {
        Task<Result<PaymentProcessResultDto>> Process(string paymentId, ProcessPaymentRequestModel processPaymentModel, CancellationToken cancellationToken = default);

        Task<Result<PaymentRequestDto>> Complete(string sessionId, CancellationToken cancellationToken = default); 

        Task<Result<PaymentRequestDto>> Refund(string paymentId, CancellationToken cancellationToken = default);

    }
}
