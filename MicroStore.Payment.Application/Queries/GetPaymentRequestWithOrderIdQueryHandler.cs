﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions.Common;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Payment.Application.Queries
{
    public class GetPaymentRequestWithOrderIdQueryHandler
        : QueryHandler<GetPaymentRequestWithOrderIdQuery>
    {

        private readonly IPaymentDbContext _paymentDbContext;

        public GetPaymentRequestWithOrderIdQueryHandler( IPaymentDbContext paymentDbContext)
        {
            
            _paymentDbContext = paymentDbContext;
        }

        public override async Task<ResponseResult> Handle(GetPaymentRequestWithOrderIdQuery request, CancellationToken cancellationToken)
        {
            var query = _paymentDbContext.PaymentRequests
                .AsNoTracking()
                .ProjectTo<PaymentRequestDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderId == request.OrderId, cancellationToken);

            if (result == null)
            {
                var error = new ErrorInfo
                {
                    Message = $"Payment request with order id : {request.OrderId} is not exist"
                };

                return Failure(HttpStatusCode.NotFound, error);
            }


            return Success(HttpStatusCode.OK, result);
        }
    }
}