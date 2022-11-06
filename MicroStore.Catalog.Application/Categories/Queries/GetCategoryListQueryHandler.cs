using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Categories.Queries
{
    internal class GetCategoryListQueryHandler : QueryHandler<GetCategoryListQuery, List<CategoryDto>>
    {
        private readonly ICatalogDbContext _catalogDbContext;


        public GetCategoryListQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;

        }

        public override async Task<List<CategoryDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var result = await _catalogDbContext.Categories
                .ToListAsync();

            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(result);
        }


    }
}
