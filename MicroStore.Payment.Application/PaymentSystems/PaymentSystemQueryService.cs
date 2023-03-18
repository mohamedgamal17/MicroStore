﻿using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Common;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.PaymentSystems
{
    public class PaymentSystemQueryService : PaymentApplicationService, IPaymentSystemQueryService
    {
        private readonly IPaymentDbContext _paymentDbContext;

        public PaymentSystemQueryService(IPaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        public async Task<ResultV2<PaymentSystemDto>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentSystems
                .AsNoTracking()
                .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result == null)
            {
                return new ResultV2<PaymentSystemDto>(new EntityNotFoundException(typeof(PaymentSystem), id));
            }

            return result;
        }

        public async Task<ResultV2<PaymentSystemDto>> GetBySystemNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentSystems
               .AsNoTracking()
               .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);

            if (result == null)
            {
                return new ResultV2<PaymentSystemDto>(new EntityNotFoundException($"payment sytem with name : {name} is not exist"));
            }

            return result;
        }

        public async Task<ResultV2<List<PaymentSystemDto>>> ListPaymentSystemAsync(CancellationToken cancellationToken = default)
        {
            var query = _paymentDbContext.PaymentSystems
              .AsNoTracking()
              .ProjectTo<PaymentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.ToListAsync(cancellationToken);

            return result;
        }
    }
}
