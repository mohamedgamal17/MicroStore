using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class RemoveProductCategoryCommandHandler : CommandHandler<RemoveProductCategoryCommand, ProductDto>
    {

        private readonly IRepository<Product> _productRepository;

        public RemoveProductCategoryCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ProductDto> Handle(RemoveProductCategoryCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.ProductId);
            }

            if (!product.ProductCategories.Any(x=> x.CategoryId == request.CategoryId))
            {
                throw new EntityNotFoundException(typeof(ProductCategory), request.CategoryId);
            }

            product.RemoveProductCategory(request.CategoryId);

            await _productRepository.UpdateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }
    }
}
