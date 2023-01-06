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
    public class GetProductWithExternalIdQueryHandler : QueryHandler<GetProductWithExternalIdQuery>
    {
        private readonly IInventoyDbContext _invetoryDbContext;

        public GetProductWithExternalIdQueryHandler( IInventoyDbContext invetorDbContext)
        {
            _invetoryDbContext = invetorDbContext;
        }

        public override async Task<ResponseResult> Handle(GetProductWithExternalIdQuery request, CancellationToken cancellationToken)
        {
            var query = _invetoryDbContext.Products
                .AsNoTracking()
                .ProjectTo<ProductDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.ExternalProductId == request.ExternalProductId);

            if(result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"product with external product id {request.ExternalProductId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
