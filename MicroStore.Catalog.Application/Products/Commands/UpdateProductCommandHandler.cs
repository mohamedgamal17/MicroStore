using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class UpdateProductCommandHandler : CommandHandler<UpdateProductCommand,ProductDto>
    {

        private readonly IRepository<Product> _productRepository;

        private readonly IImageService _imageService;

        public UpdateProductCommandHandler(IRepository<Product> productRepository, IImageService imageService)
        {
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public override async Task<ResponseResult<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository
            .SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                return Failure(HttpStatusCode.NotFound, 
                    new ErrorInfo { Message = $"Product entity with id : {request.ProductId} is not found" });
            }

            product.Sku = request.Sku;

            product.Name = request.Name;

            product.Price = request.Price;

            product.ShortDescription = request.ShortDescription;

            product.LongDescription = request.LongDescription;

            product.OldPrice = request.OldPrice;
   
            if(request.Weight != null)
            {

                product.Weight = request.Weight.AsWeight();
            }

            if (request.Dimensions != null)
            {
                product.Dimensions = request.Dimensions.AsDimension();
            }

            if (request.Thumbnail != null)
            {
                var imageResult = await _imageService.SaveAsync(request.Thumbnail,cancellationToken);

                if (!imageResult.IsValid)
                {
                    return Failure(HttpStatusCode.BadRequest, "Image extension is not valid");
                }

                product.Thumbnail = imageResult.ImageLink;
            }

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Product, ProductDto>(product)) ;
        }

    }
}
