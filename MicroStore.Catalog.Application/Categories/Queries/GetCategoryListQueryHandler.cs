using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using MicroStore.Catalog.Application.Abstractions.Common;
using System.Net;
using Volo.Abp.AutoMapper;

namespace MicroStore.Catalog.Application.Categories.Queries
{
    internal class GetCategoryListQueryHandler : QueryHandler<GetCategoryListQuery>
    {
        private readonly ICatalogDbContext _catalogDbContext;

        private readonly IMapperAccessor _mapperAccessor;
        public GetCategoryListQueryHandler(ICatalogDbContext catalogDbContext,  IMapperAccessor mapperAccessor)
        {
            _catalogDbContext = catalogDbContext;
            _mapperAccessor = mapperAccessor;
        }

        public override async Task<ResponseResult> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {

            var query = _catalogDbContext.Categories
                .AsNoTracking()
                .ProjectTo<CategoryListDto>(_mapperAccessor.Mapper.ConfigurationProvider);

            if(request.SortBy != null)
            {
                query = TryToSort(query,request.SortBy,request.Desc);
            }

            var result = await query.ToListAsync(cancellationToken);

            return Success(HttpStatusCode.OK, result);

        }


        private IQueryable<CategoryListDto> TryToSort(IQueryable<CategoryListDto> query , string sortBy , bool desc)
        {
            return sortBy.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                _ => query
            };
        }


    }
}
