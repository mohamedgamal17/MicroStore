using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Commands
{
    public class AdjustProductInventoryCommandHandler : CommandHandler<AdjustProductInventoryCommand, ProductAdjustedInventoryDto>
    {

        private readonly IRepository<Product> _productRepository;

        public AdjustProductInventoryCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ProductAdjustedInventoryDto> Handle(AdjustProductInventoryCommand request, CancellationToken cancellationToken)
        {
           Product? product  = await  _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if(product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.ProductId);
            }

            product.AdjustInventory(request.Stock, request.Reason);


            await _productRepository.UpdateAsync(product);


            return new ProductAdjustedInventoryDto
            {
                ProductId = request.ProductId,
                Stock = product.Stock
            };
        }
    }
}
