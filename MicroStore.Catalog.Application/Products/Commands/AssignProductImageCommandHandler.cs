using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Entities;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.BuildingBlocks.Results;
using System.Net;
using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.Catalog.Application.Products.Commands
{
    public class AssignProductImageCommandHandler : CommandHandlerV1<AssignProductImageCommand>
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
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Product entity with id : {request.ProductId} is not found"
                });
            }

            var imageResult = await _imageService.SaveAsync(request.ImageModel);

            product.AssignProductImage(imageResult.ImageLink, request.DisplayOrder);

            await _productRepository.UpdateAsync(product);

            return ResponseResult.Success((int)HttpStatusCode.Created, ObjectMapper.Map<Product, ProductDto>(product));

        }
    }
}
