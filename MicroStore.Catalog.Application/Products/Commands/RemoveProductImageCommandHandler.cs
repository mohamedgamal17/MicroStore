using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class RemoveProductImageCommandHandler : CommandHandlerV1<RemoveProductImageCommand>
    {
        private readonly IRepository<Product> _productRepository;

        public RemoveProductImageCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ResponseResult> Handle(RemoveProductImageCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if(product == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Product entity with id : {request.ProductId} is not found"
                });
            }

            if (!product.ProductImages.Any(x => x.Id == request.ProductImageId))
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Product image entity with id : {request.ProductImageId} is not found"
                });
            }

            product.RemoveProductImage(request.ProductImageId);

            await _productRepository.UpdateAsync(product);

            return  ResponseResult.Success((int) HttpStatusCode.Accepted, ObjectMapper.Map<Product, ProductDto>(product)) ;
        }
    }
}
