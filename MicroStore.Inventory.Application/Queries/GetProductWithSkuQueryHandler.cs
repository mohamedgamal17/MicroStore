using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Common;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Inventory.Application.Queries
{
    public class GetProductWithSkuQueryHandler : QueryHandler<GetProductWithSkuQuery, ProductDto>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public GetProductWithSkuQueryHandler( IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public override async Task<ResponseResult<ProductDto>> Handle(GetProductWithSkuQuery request, CancellationToken cancellationToken)
        {
            var query =  _inventoryDbContext.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Sku == request.Sku, cancellationToken);

            if(result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"product with sku {request.Sku} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
