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
    public class GetOrderQueryHandler : QueryHandler<GetOrderQuery,OrderDto>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public GetOrderQueryHandler( IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public override async Task<ResponseResult<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
              .AsNoTracking()
              .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);

           
            var result =  await query.SingleOrDefaultAsync(x=> x.Id== request.OrderId);

            if (result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"order with id {request.OrderId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
