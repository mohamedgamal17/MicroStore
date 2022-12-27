using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IImageService _imageService;

        public CreateProductCommandHandler(IRepository<Product> productRepository, IImageService imageService)
        {
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public override async Task<ResponseResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            ImageResult imageResult = await _imageService.SaveAsync(request.ImageModel);

            Product product = new Product(request.Sku, request.Name, request.Price,imageResult.ImageLink);

            product.ShortDescription = request.ShortDescription;

            product.LongDescription = request.LongDescription;

            product.OldPrice = request.OldPrice;

            product.Weight = request.Weight?.AsWeight() ?? Weight.Empty;

            product.Dimensions = request.Dimensions?.AsDimension() ?? Dimension.Empty;

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return ResponseResult.Success((int) HttpStatusCode.Created , ObjectMapper.Map<Product, ProductDto>(product)) ;
        }


    }
}
