using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using System.Net;
namespace MicroStore.Catalog.Application.Products.Queries
{
    internal class GetProductQueryHandler : QueryHandler<GetProductQuery,ProductDto>
    {
        private readonly ICatalogDbContext _catalogDbContext;

        public GetProductQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public override async Task<ResponseResult<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = _catalogDbContext.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var product = await query.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if(product == null)
            {
                return Failure(HttpStatusCode.NotFound, $"Product entity with id : {request.Id} is not found");
            }
            
            return Success(HttpStatusCode.OK , product) ;
        }


    }
}
