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
    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand,ProductDto>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IImageService _imageService;

        public CreateProductCommandHandler(IRepository<Product> productRepository, IImageService imageService)
        {
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public override async Task<ResponseResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            

            Product product = new Product(request.Sku, request.Name, request.Price);

            product.ShortDescription = request.ShortDescription;

            product.LongDescription = request.LongDescription;

            product.OldPrice = request.OldPrice;

            product.Weight = request.Weight?.AsWeight() ?? Weight.Empty;

            product.Dimensions = request.Dimensions?.AsDimension() ?? Dimension.Empty;

            if (request.Thumbnail != null)
            {
                ImageResult imageResult = await _imageService.SaveAsync(request.Thumbnail);

                if (!imageResult.IsValid)
                {
                    return Failure(HttpStatusCode.BadRequest, "Invalid image extension");
                }

                product.Thumbnail = imageResult.ImageLink;
            }

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.Created , ObjectMapper.Map<Product, ProductDto>(product)) ;
        }


    }
}
