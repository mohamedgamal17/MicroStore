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
    public class GetOrderWithExternalIdQueryHandler : QueryHandler<GetOrderWithExternalIdQuery,OrderDto>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public GetOrderWithExternalIdQueryHandler(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public override async Task<ResponseResult<OrderDto>> Handle(GetOrderWithExternalIdQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
              .AsNoTracking()
              .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.ExternalOrderId == request.ExternalOrderId);

            if (result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"order with external id {request.ExternalOrderId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
