﻿using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
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

        public async Task<UnitResultV2<PaymentRequestDto>> CreateAsync(CreatePaymentRequestModel model, CancellationToken cancellationToken = default)
        {
            bool isOrderPaymentCreated = await _paymentRequestRepository.AnyAsync(x => x.OrderId == model.OrderId
               || x.OrderNumber == model.OrderNumber);

            if (isOrderPaymentCreated)
            {

                return UnitResultV2.Failure<PaymentRequestDto>(ErrorInfo.BusinessLogic($"Order payment request for order id : {model.OrderId} , with number : {model.OrderNumber} is already created"));

            }


            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNumber,
                CustomerId = model.UserId,
                ShippingCost = model.ShippingCost,
                TaxCost = model.TaxCost,
                SubTotal = model.SubTotal,
                TotalCost = model.TotalCost,
                Items = PrepareOrderItems(model.Items)
            };

            await _paymentRequestRepository.InsertAsync(paymentRequest);

            var result = ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);

            return UnitResultV2.Success(result);
        }

        public async Task<UnitResultV2<PaymentProcessResultDto>> ProcessPaymentAsync(string paymentId, ProcessPaymentRequestModel model, CancellationToken cancellationToken = default)
        {
            var paymentRequest = await _paymentRequestRepository.SingleOrDefaultAsync(x => x.Id == paymentId, cancellationToken);

            if (paymentRequest == null)
            {
                return UnitResultV2.Failure<PaymentProcessResultDto>(ErrorInfo.NotFound($"Payment request with id :{paymentId}, is not exist"));
            }

            if (paymentRequest.State != PaymentStatus.Waiting)
            {

                return UnitResultV2.Failure<PaymentProcessResultDto>(
                    ErrorInfo.BusinessLogic($"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                    $"in  {PaymentStatus.Waiting}"));
            }

            var unitResult = await _paymentMethodResolver.Resolve(model.GatewayName, cancellationToken);

            if (unitResult.IsFailure)
            {
                return UnitResultV2.Failure<PaymentProcessResultDto>(unitResult.Error);
            }

            var paymentMethod = unitResult.Result;


            return await paymentMethod.Process(paymentId, model);
        }

        public async Task<UnitResultV2<PaymentRequestDto>> RefundPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository
                 .SingleOrDefaultAsync(x => x.Id == paymentId);

            if (paymentRequest == null)
            {
                return UnitResultV2.Failure<PaymentRequestDto>(ErrorInfo.NotFound($"Payment request with id :{paymentId}, is not exist"));

            }

            if (paymentRequest.State != PaymentStatus.Payed)
            {

                return UnitResultV2.Failure<PaymentRequestDto>(ErrorInfo.BusinessLogic($"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                 $"in  {PaymentStatus.Payed}"));
            }


            var unitResult = await _paymentMethodResolver.Resolve(paymentRequest.PaymentGateway!);

            if (unitResult.IsFailure)
            {
                return UnitResultV2.Failure<PaymentRequestDto>(unitResult.Error);
            }

            var paymentMethod = unitResult.Result;

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
