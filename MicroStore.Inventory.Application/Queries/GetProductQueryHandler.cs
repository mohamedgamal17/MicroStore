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
    public class GetProductQueryHandler : QueryHandler<GetProductQuery>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public GetProductQueryHandler(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public override async Task<ResponseResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Products
             .AsNoTracking()
             .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if (result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"product with product id {request.ProductId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
