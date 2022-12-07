using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class RemoveProductImageCommandHandler : CommandHandler<RemoveProductImageCommand, ProductDto>
    {
        private readonly IRepository<Product> _productRepository;

        public RemoveProductImageCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ProductDto> Handle(RemoveProductImageCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if(product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.ProductId);
            }

            if (!product.ProductImages.Any(x => x.Id == request.ProductImageId))
            {
                throw new EntityNotFoundException(typeof(ProductImage), request.ProductImageId);
            }

            product.RemoveProductImage(request.ProductImageId);

            await _productRepository.UpdateAsync(product);

            return ObjectMapper.Map<Product,ProductDto>(product);
        }
    }
}
