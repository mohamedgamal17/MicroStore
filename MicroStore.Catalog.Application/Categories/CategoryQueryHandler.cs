
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Categories
{
    internal class CategoryQueryHandler : RequestHandler,
        IQueryHandler<GetCategoryListQuery, ListResultDto<CategoryListDto>>,
        IQueryHandler<GetCategoryQuery,CategoryDto>
    {
        private readonly ICatalogDbContext _catalogDbContext;

        public CategoryQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<ResponseResult<ListResultDto<CategoryListDto>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var query = _catalogDbContext.Categories
            .AsNoTracking()
                .ProjectTo<CategoryListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (request.SortBy != null)
            {
                query = TryToSort(query, request.SortBy, request.Desc);
            }

            var result = await query.ToListAsync(cancellationToken);

            return Success(HttpStatusCode.OK, result);

        }


  

        public async Task<ResponseResult<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            Category? category = await _catalogDbContext.Categories
                 .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return Failure<CategoryDto>(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Category with id : {request.Id} is not exist"
                });
            }

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Category, CategoryDto>(category));
        }

        private IQueryable<CategoryListDto> TryToSort(IQueryable<CategoryListDto> query, string sortBy, bool desc)
        {
            return sortBy.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                _ => query
            };
        }
    }
}
