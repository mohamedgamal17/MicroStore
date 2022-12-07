using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IImageService _imageService;

        public CreateProductCommandHandler(IRepository<Product> productRepository, IImageService imageService)
        {
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public override async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            ImageResult imageResult = await _imageService.SaveAsync(request.ImageModel);

            Product product = new Product(request.Sku, request.Name, request.Price,imageResult.ImageLink);

            product.ShortDescription = request.ShortDescription;

            product.LongDescription = request.LongDescription;

            product.OldPrice = request.OldPrice;

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }


    }
}
