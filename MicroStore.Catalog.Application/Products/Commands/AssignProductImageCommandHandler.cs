using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.BuildingBlocks.Results;
using System.Net;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class AssignProductImageCommandHandler : CommandHandler<AssignProductImageCommand>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IBlobContainer _blobContainer;

        private readonly IImageService _imageService;
        public AssignProductImageCommandHandler(IRepository<Product> productRepository, IBlobContainer blobContainer, 
            IImageService imageService)
        {
            _productRepository = productRepository;
            _blobContainer = blobContainer;
            _imageService = imageService;
        }

        public override async Task<ResponseResult> Handle(AssignProductImageCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if(product == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Product entity with id : {request.ProductId} is not found");
            }

            var imageResult = await _imageService.SaveAsync(request.ImageModel);

            if (!imageResult.IsValid)
            {
                return Failure(HttpStatusCode.BadRequest, "Invalid image extension");
            }

            product.AssignProductImage(imageResult.ImageLink, request.DisplayOrder);

            await _productRepository.UpdateAsync(product);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<Product, ProductDto>(product));

        }
    }
}
