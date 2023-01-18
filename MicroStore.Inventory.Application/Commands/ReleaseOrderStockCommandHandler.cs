using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Commands
{
    public class ReleaseOrderStockCommandHandler : CommandHandler<ReleaseOrderStockCommand>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Order> _orderRepository;

        public ReleaseOrderStockCommandHandler(IRepository<Product> productRepository, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public override async Task<ResponseResult<Unit>> Handle(ReleaseOrderStockCommand request, CancellationToken cancellationToken)
        {

            var query = await _orderRepository.WithDetailsAsync(x=> x.Items);

            var order = await query.SingleAsync(x => x.ExternalOrderId == request.ExternalOrderId);

            foreach (var orderItem in order.Items)
            {
                Product product = await _productRepository.SingleAsync(x => x.ExternalProductId == orderItem.ExternalProductId);

                product.ReleaseStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product);
            }

            order.IsCancelled = true;

            await _orderRepository.UpdateAsync(order);

            return ResponseResult.Success((int) HttpStatusCode.OK);
        }
    }
}
