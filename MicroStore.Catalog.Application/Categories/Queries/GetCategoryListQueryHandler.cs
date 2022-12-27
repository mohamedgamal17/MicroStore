using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Categories.Queries;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Categories.Queries
{
    internal class GetCategoryListQueryHandler : QueryHandler<GetCategoryListQuery>
    {
        private readonly ICatalogDbContext _catalogDbContext;


        public GetCategoryListQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;

        }

        public override async Task<ResponseResult> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var result = await _catalogDbContext.Categories
            .ToListAsync();

            var mapping = ObjectMapper.Map<List<Category>, List<CategoryDto>>(result);

            return ResponseResult.Success((int)HttpStatusCode.OK, mapping);
        }


    }
}
