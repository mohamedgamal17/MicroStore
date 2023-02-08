using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Orders
{
    public class OrderCommandHandler : RequestHandler,
        ICommandHandler<AllocateOrderStockCommand>,
        ICommandHandler<ReleaseOrderStockCommand>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Order> _orderRepository;

        public OrderCommandHandler(IRepository<Product> productRepository, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ResponseResult<Unit>> Handle(AllocateOrderStockCommand request, CancellationToken cancellationToken)
        {
            List<Result> failureResults = new List<Result>();



            foreach (var orderItem in request.Items)
            {
                Product product = await _productRepository.SingleOrDefaultAsync(x => x.ExternalProductId
                == orderItem.ExternalProductId);

                var result = product.CanAllocateStock(orderItem.Quantity);

                if (result.IsFailure)
                {
                    failureResults.Add(result);
                }
            }

            if (failureResults.Any())
            {

                string details = failureResults.Select(x => x.ToString())
                        .Aggregate((t1, t2) => t1 + "\n" + t2)!;

                var errorInfo = new ErrorInfo
                {
                    Message = details,
                };

                return ResponseResult.Failure((int)HttpStatusCode.BadRequest, errorInfo);
            }

            foreach (var orderItem in request.Items)
            {
                Product product = await _productRepository.SingleOrDefaultAsync(x => x.ExternalProductId == orderItem.ExternalProductId);

                product.AllocateStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);
            }

            Order order = new Order
            {
                ExternalOrderId = request.ExternalOrderId,
                OrderNumber = request.OrderNumber,
                ExternalPaymentId = request.ExternalPaymentId,
                UserId = request.UserId,
                ShippingAddress = request.ShippingAddress.AsAddressValueObject(),
                BillingAddres = request.ShippingAddress.AsAddressValueObject(),
                ShippingCost = request.ShippingCost,
                TaxCost = request.TaxCost,
                SubTotal = request.SubTotal,
                TotalPrice = request.TotalPrice,
                Items = request.Items.Select(x => new OrderItem
                {

                    ExternalItemId = x.ExternalItemId,
                    ExternalProductId = x.ExternalProductId,
                    Name = x.Name,
                    Sku = x.Sku,
                    Thumbnail = x.Thumbnail,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,

                }).ToList()
            };

            await _orderRepository.InsertAsync(order);

            return ResponseResult.Success((int)HttpStatusCode.OK);
        }

        public async Task<ResponseResult<Unit>> Handle(ReleaseOrderStockCommand request, CancellationToken cancellationToken)
        {
            var query = await _orderRepository.WithDetailsAsync(x => x.Items);

            var order = await query.SingleAsync(x => x.ExternalOrderId == request.ExternalOrderId);

            foreach (var orderItem in order.Items)
            {
                Product product = await _productRepository.SingleAsync(x => x.ExternalProductId == orderItem.ExternalProductId);

                product.ReleaseStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product);
            }

            order.IsCancelled = true;

            await _orderRepository.UpdateAsync(order);

            return ResponseResult.Success((int)HttpStatusCode.OK);
        }

       
    }
}
