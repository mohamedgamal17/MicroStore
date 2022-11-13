using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Commands
{
    public class ReciveProductCommandHandler : CommandHandler<ReciveProductCommand, ProductRecivedDto>
    {

        private readonly IRepository<Product> _productRepository;

        public ReciveProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ProductRecivedDto> Handle(ReciveProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if(product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.ProductId);
            }


            product.ReciveQuantity(request.RecivedQuantity);


            return new ProductRecivedDto
            {
                ProductId = product.Id,
                Stock = product.Stock,
                RecivedStock = request.RecivedQuantity
            };
        }
    }
}
