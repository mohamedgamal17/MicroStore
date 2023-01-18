using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class AssignProductCategoryComandHandler : CommandHandler<AssignProductCategoryCommand,ProductDto>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Category> _categoryRepository;

        public AssignProductCategoryComandHandler(IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public override async Task<ResponseResult<ProductDto>> Handle(AssignProductCategoryCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if(product == null)
            {
                return Failure(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Product entity with id : {request.ProductId} is not found"
                });
            }

            Category? category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

            if(category == null)
            {
                return Failure(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Category entity with id : {request.CategoryId} is not found"
                });
            }

            product.AddOrUpdateProductCategory(category, request.IsFeatured);

            await _productRepository.UpdateAsync(product);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<Product, ProductDto>(product));
        }
    }
}
