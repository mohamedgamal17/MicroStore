using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
namespace MicroStore.Catalog.Application.Categories.Queries
{
    internal class GetCategoryQueryHandler : QueryHandler<GetCategoryQuery,CategoryDto>
    {

        private readonly ICatalogDbContext _catalogDbContext;

        public GetCategoryQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }
        public override async Task<ResponseResult<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            Category? category = await _catalogDbContext.Categories
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return Failure(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Category with id : {request.Id} is not exist"
                });
            }

            return Success( HttpStatusCode.OK , ObjectMapper.Map<Category, CategoryDto>(category));
        }


    }
}
