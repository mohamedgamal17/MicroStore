using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Application.Dtos;
using System.Net;

namespace MicroStore.Inventory.Application.Products
{
    public class ProductQueryHandler : RequestHandler,
        IQueryHandler<GetProductListQuery, PagedResult<ProductDto>>,
        IQueryHandler<GetProductWithExternalIdQuery, ProductDto>,
        IQueryHandler<GetProductWithSkuQuery, ProductDto>,
        IQueryHandler<GetProductQuery,ProductDto>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public ProductQueryHandler(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<ResponseResult<PagedResult<ProductDto>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Products
             .AsNoTracking()
             .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<ProductDto>> Handle(GetProductWithExternalIdQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Products
                  .AsNoTracking()
                  .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.ExternalProductId == request.ExternalProductId);

            if (result == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound, $"order with external id {request.ExternalProductId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<ProductDto>> Handle(GetProductWithSkuQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Products
               .AsNoTracking()
               .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Sku == request.Sku, cancellationToken);

            if (result == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound, $"product with sku {request.Sku} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }

        public async Task<ResponseResult<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if (result == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound, $"product with product id {request.ProductId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
