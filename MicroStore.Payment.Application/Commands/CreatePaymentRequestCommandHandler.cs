using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Domain;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Commands
{
    public class CreatePaymentRequestCommandHandler : CommandHandlerV1<CreatePaymentRequestCommand>
    {

        private readonly IRepository<PaymentRequest> _paymentRepository;

        public CreatePaymentRequestCommandHandler(IRepository<PaymentRequest> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public override async Task<ResponseResult> Handle(CreatePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            bool isOrderPaymentCreated = await _paymentRepository.AnyAsync(x=> x.OrderId == request.OrderId
              || x.OrderNumber == request.OrderNumber);

            if (isOrderPaymentCreated)
            {

                return ResponseResult.Failure((int)HttpStatusCode.BadRequest,new ErrorInfo {
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
                Items = MapOrderItems(request.Items)
            };

            await _paymentRepository.InsertAsync(paymentRequest);

            var result = ObjectMapper.Map<PaymentRequest, PaymentRequestCreatedDto>(paymentRequest);

            return ResponseResult.Success((int) HttpStatusCode.Created, result);
        }



        private List<PaymentRequestProduct> MapOrderItems(List<OrderItemModel> items)
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
