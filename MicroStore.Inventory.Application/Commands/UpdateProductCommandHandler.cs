using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Commands
{
    public class UpdateProductCommandHandler : CommandHandler<UpdateProdutCommand>
    {
        private readonly IRepository<Product> _productRepository;

        public UpdateProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<Unit> Handle(UpdateProdutCommand request, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.SingleAsync(x => x.ExternalProductId == request.ExternalProductId);
            product.Sku = request.Sku;
            product.Name = request.Name;
            product.Thumbnail = request.Thumbnail;

            await _productRepository.UpdateAsync(product);

            return Unit.Value;
        }
    }
}
