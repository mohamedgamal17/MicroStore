using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Categories.Queries
{
    internal class GetCategoryQueryHandler : QueryHandler<GetCategoryQuery, CategoryDto>
    {

        private readonly ICatalogDbContext _catalogDbContext;



        public GetCategoryQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;


        }

        public override async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            Category? category = await _catalogDbContext.Categories
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                throw new EntityNotFoundException(typeof(Category), request.Id);
            }

            return ObjectMapper.Map<Category, CategoryDto>(category);
        }


    }
}
