using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Common;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;
namespace MicroStore.Inventory.Application.Queries
{
    public class GetUserOrderListQueryHandler : QueryHandler<GetUserOrderListQuery>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public GetUserOrderListQueryHandler(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public override async Task<ResponseResult> Handle(GetUserOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
                .AsNoTracking()
                .ProjectTo<OrderListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.Where(x=> x.UserId == request.UserId)
                .PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }
    }
}
