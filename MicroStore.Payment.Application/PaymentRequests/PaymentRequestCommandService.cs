﻿using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Payment.Application.PaymentRequests
{
    public class PaymentRequestCommandService : PaymentApplicationService, IPaymentRequestCommandService
    {
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;
        private readonly IPaymentMethodResolver _paymentMethodResolver;

        public PaymentRequestCommandService(IRepository<PaymentRequest> paymentRequestRepository, IPaymentMethodResolver paymentMethodResolver)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _paymentMethodResolver = paymentMethodResolver;
        }

        public async Task<ResultV2<PaymentRequestDto>> CreateAsync(CreatePaymentRequestModel model, CancellationToken cancellationToken = default)
        {
            bool isOrderPaymentCreated = await _paymentRequestRepository.AnyAsync(x => x.OrderId == model.OrderId
               || x.OrderNumber == model.OrderNumber);

            if (isOrderPaymentCreated)
            {
                return new ResultV2<PaymentRequestDto>(new BusinessException($"Order payment request for order id : {model.OrderId} , with number : {model.OrderNumber} is already created"));
            }


            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNumber,
                UserId = model.UserId,
                ShippingCost = model.ShippingCost,
                TaxCost = model.TaxCost,
                SubTotal = model.SubTotal,
                TotalCost = model.TotalCost,
                Items = PrepareOrderItems(model.Items)
            };

            await _paymentRequestRepository.InsertAsync(paymentRequest);

            var result = ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);

            return result;
        }

        public async Task<ResultV2<PaymentProcessResultDto>> ProcessPaymentAsync(string paymentId, ProcessPaymentRequestModel model, CancellationToken cancellationToken = default)
        {
            var paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);

            if (paymentRequest == null)
            {
                return new ResultV2<PaymentProcessResultDto>(new EntityNotFoundException(typeof(PaymentRequest), paymentId));
            }

            if (paymentRequest.State != PaymentStatus.Waiting)
            {


                return new ResultV2<PaymentProcessResultDto>(new BusinessException($"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                    $"in  {PaymentStatus.Waiting}"));
            }

            var unitResult = await _paymentMethodResolver.Resolve(model.GatewayName, cancellationToken);

            if (unitResult.IsFailure)
            {
                return new ResultV2<PaymentProcessResultDto>(unitResult.Exception);
            }

            var paymentMethod = unitResult.Value;


            return await paymentMethod.Process(paymentId, model);
        }

        public async Task<ResultV2<PaymentRequestDto>> RefundPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository
                 .SingleOrDefaultAsync(x => x.Id == paymentId);

            if (paymentRequest == null)
            {
                return new ResultV2<PaymentRequestDto>(new EntityNotFoundException(typeof(PaymentRequest), paymentId));

            }

            if (paymentRequest.State != PaymentStatus.Payed)
            {

                return new ResultV2<PaymentRequestDto>(new BusinessException($"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                 $"in  {PaymentStatus.Payed}"));
            }


            var unitResult = await _paymentMethodResolver.Resolve(paymentRequest.PaymentGateway!);

            if (unitResult.IsFailure)
            {
                return new ResultV2<PaymentRequestDto>(unitResult.Exception);
            }

            var paymentMethod = unitResult.Value;

            return await paymentMethod.Refund(paymentId, cancellationToken);
        }

        private List<PaymentRequestProduct> PrepareOrderItems(List<PaymentProductModel> items)
        {
            return items.Select(x => new PaymentRequestProduct
            {
                ProductId = x.ProductId,
                Sku = x.Sku,
                Name = x.Name,
                Thumbnail = x.Image,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList();
        }
    }
}
