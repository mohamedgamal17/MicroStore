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
    public class RemoveProductCategoryCommandHandler : CommandHandler<RemoveProductCategoryCommand>
    {

        private readonly IRepository<Product> _productRepository;

        public RemoveProductCategoryCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ResponseResult> Handle(RemoveProductCategoryCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if (product == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Product entity with id : {request.ProductId} is not found"
                });
            }

            if (!product.ProductCategories.Any(x=> x.CategoryId == request.CategoryId))
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Product category entity with id : {request.CategoryId} is not found"
                });
            }

            product.RemoveProductCategory(request.CategoryId);

            await _productRepository.UpdateAsync(product);

            return ResponseResult.Success((int) HttpStatusCode.Accepted, ObjectMapper.Map<Product, ProductDto>(product));
        }
    }
}
