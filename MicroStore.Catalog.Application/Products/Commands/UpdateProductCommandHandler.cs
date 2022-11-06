using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Products.Commands
{
    public class UpdateProductCommandHandler : CommandHandler<UpdateProductCommand, ProductDto>
    {

        private readonly IRepository<Product> _productRepository;

        public UpdateProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;

        }

        public override async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository
            .SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.ProductId);
            }

            product.AdjustProductSku(request.Sku);

            product.AdjustProductName(request.Name);

            product.AdjustProductPrice(request.Price);

            product.SetProductShortDescription(request.ShortDescription);

            product.SetProductLongDescription(request.LongDescription);

            product.SetProductOldPrice(request.OldPrice);

            request.ProductCategories.ForEach((productCategory) =>
            {
                product.AddOrUpdateProductCategory(productCategory.CategoryId, productCategory.IsFeatured);
            });

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

    }
}
