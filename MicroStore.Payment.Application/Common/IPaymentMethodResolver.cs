﻿using MicroStore.BuildingBlocks.Results;
using MicroStore.Payment.Domain.Shared;

namespace MicroStore.Payment.Application.Abstractions
{
    public interface IPaymentMethodResolver
    {
        Task<UnitResultV2<IPaymentMethod>> Resolve(string paymentGatewayName, CancellationToken cancellationToken  = default);
    }
}