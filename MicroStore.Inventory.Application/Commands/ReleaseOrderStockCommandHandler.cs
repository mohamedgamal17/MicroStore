using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Commands
{
    public class ReleaseOrderStockCommandHandler : CommandHandler<ReleaseOrderStockCommand>
    {
        private readonly IRepository<Product> _productRepository;

        public ReleaseOrderStockCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<Unit> Handle(ReleaseOrderStockCommand request, CancellationToken cancellationToken)
        {
            foreach (var orderItem in request.Products)
            {
                Product product = await _productRepository.SingleAsync(x => x.Id == orderItem.ProductId);

                product.ReleaseStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product);
            }


            return Unit.Value;
        }
    }
}
