using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Common;
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

        private readonly IImageService _imageService;

        public UpdateProductCommandHandler(IRepository<Product> productRepository, IImageService imageService)
        {
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public override async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository
            .SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.ProductId);
            }

            product.Sku = request.Sku;

            product.Name = request.Name;

            product.Price = request.Price;

            product.ShortDescription = request.ShortDescription;

            product.LongDescription = request.LongDescription;

            product.OldPrice = request.OldPrice;

            if(request.ImageModel != null)
            {
                var imageResult = await _imageService.SaveAsync(request.ImageModel,cancellationToken);

                product.Thumbnail = imageResult.ImageLink;
            }

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

    }
}
