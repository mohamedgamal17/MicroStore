using FluentValidation;
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

        public PaymentGrpcService(IPaymentRequestCommandService paymentRequestCommandService, IPaymentRequestQueryService paymentRequestQueryService)
        {
            _paymentRequestCommandService = paymentRequestCommandService;
            _paymentRequestQueryService = paymentRequestQueryService;
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
                Status = payment.Status,
                CreatedAt = payment.CreationTime.ToTimestamp(),
                Description = payment.Description,
                
            };

            payment.Items.ForEach(product =>
            {
                response.Items.Add(PreparePaymentProductRequest(product));
            });

            if(payment.CapturedAt != null)
            {
                response.CapturedAt = payment.CapturedAt.Value.ToTimestamp();
            }

            if (payment.RefundedAt != null)
            {
                response.RefundedAt = payment.RefundedAt.Value.ToTimestamp();
            }

            if (payment.FaultAt != null)
            {
                response.FaultAt = payment.FaultAt.Value.ToTimestamp();
            }

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
                AmountTotal = paymentProcess.AmountTotal
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
