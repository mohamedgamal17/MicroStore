﻿using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Payment.Api.Host.Grpc
{
    public class PaymentGrpcService : PaymentService.PaymentServiceBase
    {
        private readonly IPaymentRequestCommandService _paymentRequestCommandService;
        private readonly IPaymentRequestQueryService _paymentRequestQueryService;

        public PaymentGrpcService(IPaymentRequestCommandService paymentRequestCommandService, IPaymentRequestQueryService paymentRequestQueryService, IAbpLazyServiceProvider lazyServiceProvider)
        {
            _paymentRequestCommandService = paymentRequestCommandService;
            _paymentRequestQueryService = paymentRequestQueryService;
            LazyServiceProvider = lazyServiceProvider;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public override async Task<PaymentResponse> Create(CreatePaymentRequest request, ServerCallContext context)
        {
            var model = PreparePaymentReqeustModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _paymentRequestCommandService.CreateAsync(model);


            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }


            return PreparePaymentResponse(result.Value);
        }

        public override async Task<PaymentProcessResponse> Process(ProcessPaymentReqeust request, ServerCallContext context)
        {
            var model = PrepareProcessPaymentReqeustModel(request);


            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(request.Id,model);


            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreparePaymentProcessResponse(result.Value);
        }

        public override async Task<PaymentResponse> Complete(CompletePaymentRequest request, ServerCallContext context)
        {
            var model = PrepareCompletePaymentModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _paymentRequestCommandService.CompleteAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreparePaymentResponse(result.Value);
        }

        public override async Task<PaymentResponse> Refund(RefundPaymentRequest request, ServerCallContext context)
        {

            var result = await _paymentRequestCommandService.RefundPaymentAsync(request.PaymentRequestId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreparePaymentResponse(result.Value);
        }

        public override async Task<PaymentListResponse> GetList(PaymentListRequest request, ServerCallContext context)
        {
            var model = PreparePaymentListQueryModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _paymentRequestQueryService.ListPaymentAsync(model , request.UserId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreparePaymentListResponse(result.Value);
        }

        public override async Task<PaymentListByOrderResponse> GetListByOrderIds(PaymentListByOrderIdsRequest request, ServerCallContext context)
        {

            var result = await _paymentRequestQueryService.ListPaymentByOrderIdsAsync(request.OrderIds.ToList());

            var response = new PaymentListByOrderResponse();

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            foreach (var item in result.Value)
            {
                response.Items.Add(PreparePaymentResponse(item));
            }

            return response;
        }

        public override async Task<PaymentListByOrderResponse> GetListByOrderNumbers(PaymentListByOrderNumbersRequest request, ServerCallContext context)
        {
            var result = await _paymentRequestQueryService.ListPaymentByOrderNumbersAsync(request.OrderNumbers.ToList());

            var response = new PaymentListByOrderResponse();

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            foreach (var item in result.Value)
            {
                response.Items.Add(PreparePaymentResponse(item));
            }

            return response;
        }
        public override async Task<PaymentResponse> GetById(GetPaymentByIdReqeust request, ServerCallContext context)
        {
            var result = await _paymentRequestQueryService.GetAsync(request.Id);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreparePaymentResponse(result.Value);
        }

        public override async Task<PaymentResponse> GetByOrderId(GetPaymentByOrderIdRequest request, ServerCallContext context)
        {
            var result = await _paymentRequestQueryService.GetByOrderIdAsync(request.OrderId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreparePaymentResponse(result.Value);
        }

        public override async Task<PaymentResponse> GetByOrderNumber(GetPaymentByOrderNumberRequest request, ServerCallContext context)
        {
            var result = await _paymentRequestQueryService.GetByOrderNumberAsync(request.OrderNumber);

            if(result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreparePaymentResponse(result.Value);
        }
        private CreatePaymentRequestModel PreparePaymentReqeustModel(CreatePaymentRequest request)
        {
            var model = new CreatePaymentRequestModel
            {
                OrderId = request.OrderId,
                UserId = request.UserId,
                OrderNumber = request.OrderNumber,
                ShippingCost = request.ShippingCost,
                SubTotal = request.SubTotal,
                TaxCost = request.TaxCost,
                TotalCost = request.TotalCost,
                Items = request.Items.Select(x => new PaymentProductModel
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Sku = x.Sku,
                    Image = x.Thumbnail,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };

            return model;
        }

        private ProcessPaymentRequestModel PrepareProcessPaymentReqeustModel(ProcessPaymentReqeust reqeust)
        {
            var model = new ProcessPaymentRequestModel
            {
                GatewayName = reqeust.GatewayName,
                CancelUrl = reqeust.CancelUrl,
                ReturnUrl = reqeust.ReturnUrl
            };

            return model;
        }

        private CompletePaymentModel PrepareCompletePaymentModel(CompletePaymentRequest request)
        {
            return new CompletePaymentModel
            {
                SessionId = request.SessionId,
                GatewayName = request.GatewayName
            };
        }

        private PaymentRequestListQueryModel PreparePaymentListQueryModel(PaymentListRequest request)
        {
            var model = new PaymentRequestListQueryModel
            {
                OrderNumber = request.OrderNumber,
                Status = request.Status,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                Skip = request.Skip,
                Length = request.Length,
                SortBy = request.SortBy,
                Desc = request.Desc,
                StartDate = request.StartDate?.ToDateTime() ?? DateTime.MinValue,
                EndDate = request.EndDate?.ToDateTime() ?? DateTime.MinValue,
            };

            return model;

        }
        private PaymentResponse PreparePaymentResponse(PaymentRequestDto payment)
        {
            var response = new PaymentResponse
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                OrderNumber = payment.OrderNumber,
                UserId = payment.UserId,
                TransctionId = payment.TransctionId,
                PaymentGateway=  payment.PaymentGateway,
                ShippingCost = payment.ShippingCost,
                TaxCost = payment.TaxCost,
                SubTotal = payment.SubTotal,
                TotalCost = payment.TotalCost,
                Status = System.Enum.Parse<PaymentStatus>(payment.Status),
                CreatedAt = payment.CreationTime.ToUniversalTime().ToTimestamp(),
                Description = payment.Description ?? string.Empty,
                CapturedAt = payment.CapturedAt.ToUniversalTime().ToTimestamp(),
                RefundedAt = payment.RefundedAt.ToUniversalTime().ToTimestamp(),
                FaultAt =payment.FaultAt.ToUniversalTime().ToTimestamp(),
                ModifiedAt = payment.LastModificationTime?.ToUniversalTime().ToTimestamp()

            };


            payment.Items?.ForEach(product =>
            {
                response.Items.Add(PreparePaymentProductRequest(product));
            });


            return response;
        }

        private PaymentItemResponse PreparePaymentProductRequest(PaymentRequestProductDto product)
        {
            var response = new PaymentItemResponse
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                ProductId = product.ProductId,
                Thumbnail = product.Thumbnail,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };

            return response;
        }

        private PaymentListResponse PreparePaymentListResponse(PagedResult<PaymentRequestDto> paged)
        {
            var resposne = new PaymentListResponse
            {
                Skip = paged.Skip,
                Length = paged.Lenght,
                TotalCount = paged.TotalCount
            };


            foreach(var item in paged.Items)
            {
                resposne.Items.Add(PreparePaymentResponse(item));
            }

            return resposne;
        }

        private PaymentProcessResponse PreparePaymentProcessResponse(PaymentProcessResultDto paymentProcess)
        {
            
            var response = new PaymentProcessResponse
            {
                SessionId = paymentProcess.SessionId,
                TransactionId = paymentProcess.TransactionId,
                SuccessUrl = paymentProcess.SuccessUrl,
                CancelUrl = paymentProcess.CancelUrl,
                CheckoutLink = paymentProcess.CheckoutLink,
                AmountSubTotal = paymentProcess.AmountSubTotal,
                AmountTotal = paymentProcess.AmountTotal,
                Provider = paymentProcess.Provider
            };

            return response;
        }

        private async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            return await validator.ValidateAsync(model);
        }

        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>>();
        }
    }
}
