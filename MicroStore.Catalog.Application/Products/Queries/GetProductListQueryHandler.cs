
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using System.Net;
using AutoMapper.QueryableExtensions;
using MicroStore.BuildingBlocks.Paging.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
namespace MicroStore.Catalog.Application.Products.Queries
{
    internal class GetProductListQueryHandler : QueryHandler<GetProductListQuery>
    {

        private readonly ICatalogDbContext _catalogDbContext;
        public GetProductListQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public override async Task<ResponseResult> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var query =  _catalogDbContext.Products.AsQueryable()
                .AsNoTracking()
                .ProjectTo<ProductListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var pagingResult = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, pagingResult);
        }


        public IQueryable<ProductListDto> TryToSort(IQueryable<ProductListDto> query , string sortBy , bool desc = false)
        {
            return sortBy.ToLower() switch {
                "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "price" => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                "old_price" => desc ? query.OrderByDescending(x => x.OldPrice) : query.OrderBy(x => x.OldPrice),
                _ => query
            };
        }
    }
}
