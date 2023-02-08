using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using System.Net;

namespace MicroStore.Catalog.Application.Products
{
    public class ProductQueryHandler : RequestHandler,
        IQueryHandler<GetProductListQuery, PagedResult<ProductListDto>>,
        IQueryHandler<GetProductQuery ,ProductDto>
    {
        private readonly ICatalogDbContext _catalogDbContext;
        public ProductQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }
        public async Task<ResponseResult<PagedResult<ProductListDto>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var query = _catalogDbContext.Products.AsQueryable()
              .AsNoTracking()
              .ProjectTo<ProductListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var pagingResult = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, pagingResult);
        }

        public async Task<ResponseResult<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = _catalogDbContext.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var product = await query.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound, $"Product entity with id : {request.Id} is not found");
            }

            return Success(HttpStatusCode.OK, product);
        }
        public IQueryable<ProductListDto> TryToSort(IQueryable<ProductListDto> query, string sortBy, bool desc = false)
        {
            return sortBy.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "price" => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                "old_price" => desc ? query.OrderByDescending(x => x.OldPrice) : query.OrderBy(x => x.OldPrice),
                _ => query
            };
        }

    }
}
