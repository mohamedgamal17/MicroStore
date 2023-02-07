using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions;
using MicroStore.Payment.Application.Extensions;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Dtos;
using MicroStore.Payment.Domain.Shared.Models;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.PaymentRequests
{
    public class PaymentRequestCommandHandler : RequestHandler,
        ICommandHandler<CreatePaymentRequestCommand, PaymentRequestDto>,
        ICommandHandler<ProcessPaymentRequestCommand, PaymentProcessResultDto>,
        ICommandHandler<CompletePaymentRequestCommand, PaymentRequestDto>,
        ICommandHandler<RefundPaymentRequestCommand,PaymentRequestDto>
    {
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;
        private readonly IPaymentMethodResolver _paymentMethodResolver;

        public PaymentRequestCommandHandler(IRepository<PaymentRequest> paymentRequestRepository, IPaymentMethodResolver paymentMethodResolver)
        {
            _paymentRequestRepository = paymentRequestRepository;
            _paymentMethodResolver = paymentMethodResolver;
        }

        public async Task<ResponseResult<PaymentRequestDto>> Handle(CreatePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            bool isOrderPaymentCreated = await _paymentRequestRepository.AnyAsync(x => x.OrderId == request.OrderId
              || x.OrderNumber == request.OrderNumber);

            if (isOrderPaymentCreated)
            {

                return Failure<PaymentRequestDto>(HttpStatusCode.BadRequest, new ErrorInfo
                {
                    Message = $"Order payment request for order id : {request.OrderId} , with number : {request.OrderNumber} is already created"
                });
            }


            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = request.OrderId,
                OrderNumber = request.OrderNumber,
                CustomerId = request.UserId,
                ShippingCost = request.ShippingCost,
                TaxCost = request.TaxCost,
                SubTotal = request.SubTotal,
                TotalCost = request.TotalCost,
                Items = PrepareOrderItems(request.Items)
            };

            await _paymentRequestRepository.InsertAsync(paymentRequest);

            var result = ObjectMapper.Map<PaymentRequest, PaymentRequestDto>(paymentRequest);

            return ResponseResult.Success((int)HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<PaymentProcessResultDto>> Handle(ProcessPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var paymentRequest = await _paymentRequestRepository.SingleAsync(x=> x.Id == request.PaymentId, cancellationToken);

            if (paymentRequest == null)
            {
                return Failure<PaymentProcessResultDto>(HttpStatusCode.NotFound, new ErrorInfo { Message = $"Payment request with id :{request.PaymentId}, is not exist" });
            }

            if (paymentRequest.State != PaymentStatus.Waiting)
            {

                var errorInfo = new ErrorInfo
                {
                    Message = $"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                    $"in  {PaymentStatus.Waiting}"
                };
                return Failure<PaymentProcessResultDto>(HttpStatusCode.BadRequest, errorInfo);
            }

            var unitResult = await _paymentMethodResolver.Resolve(request.PaymentGatewayName, cancellationToken);

            if (unitResult.IsFailure)
            {
                return unitResult.ConvertFaildUnitResult<PaymentProcessResultDto>();
            }

            var paymentMethod = unitResult.Value;


            return  await paymentMethod.Process(request.PaymentId, new ProcessPaymentModel
            {
                ReturnUrl = request.ReturnUrl,
                CancelUrl = request.CancelUrl
            }, cancellationToken);

        }

        public async Task<ResponseResult<PaymentRequestDto>> Handle(CompletePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var systemResult = await _paymentMethodResolver.Resolve(request.PaymentGatewayName);

            if (systemResult.IsFailure)
            {
                return systemResult.ConvertFaildUnitResult<PaymentRequestDto>();
            }

            var paymentMethod = systemResult.Value;

            return   await paymentMethod.Complete(new CompletePaymentModel
            {
                Token = request.Token,

            }, cancellationToken);


        }

        public async Task<ResponseResult<PaymentRequestDto>> Handle(RefundPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            PaymentRequest paymentRequest = await _paymentRequestRepository
                 .SingleOrDefaultAsync(x => x.Id == request.PaymentId);

            if (paymentRequest == null)
            {
                return Failure<PaymentRequestDto>(HttpStatusCode.NotFound,
                    new ErrorInfo { Message = $"Payment request with id :{request.PaymentId}, is not exist" });
            }

            if (paymentRequest.State != PaymentStatus.Payed)
            {
                var errorInfo = new ErrorInfo
                {
                    Message = $"Invalid payment request state {paymentRequest.State}.Payment request state should be" +
                 $"in  {PaymentStatus.Payed}"
                };

                return Failure<PaymentRequestDto>(HttpStatusCode.BadRequest, errorInfo);
            }


            var unitResult = await _paymentMethodResolver.Resolve(paymentRequest.PaymentGateway!);

            var paymentMethod = unitResult.Value;

            return await paymentMethod.Refund(request.PaymentId, cancellationToken);

        }




        protected  ResponseResult<T> FromUnitResult<T>(UnitResult<T> result , HttpStatusCode successStatusCode)
        {
            if (result.IsFailure)
            {
                return result.ConvertFaildUnitResult<T>();
            }

            return Success<T>(successStatusCode, result.Value);
        }

        private List<PaymentRequestProduct> PrepareOrderItems(List<OrderItemModel> items)
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
