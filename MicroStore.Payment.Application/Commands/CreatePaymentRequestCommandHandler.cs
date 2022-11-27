using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Dtos;
using MicroStore.Payment.Domain.Shared.Domain;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Payment.Application.Commands
{
    public class CreatePaymentRequestCommandHandler : CommandHandler<CreatePaymentRequestCommand, PaymentRequestCreatedDto>
    {

        private readonly IRepository<PaymentRequest> _paymentRepository;

        public CreatePaymentRequestCommandHandler(IRepository<PaymentRequest> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public override async Task<PaymentRequestCreatedDto> Handle(CreatePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = request.OrderId,
                OrderNumber = request.OrderNubmer,
                CustomerId = request.UserId,
                ShippingCost = request.ShippingCost,
                TaxCost = request.TaxCost,
                SubTotal = request.SubtTotal,
                TotalCost = request.TotalCost,
                Items = MapOrderItems(request.Items)
            };

            await _paymentRepository.InsertAsync(paymentRequest);


            return ObjectMapper.Map<PaymentRequest, PaymentRequestCreatedDto>(paymentRequest);
        }



        private List<PaymentRequestProduct> MapOrderItems(List<OrderItemModel> items)
        {
            return items.Select(x => new PaymentRequestProduct
            {
                ProductId = x.ProductId,
                Sku = x.Sku,
                Name = x.Name,
                Image = x.Image,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList();
        }
    }
}
