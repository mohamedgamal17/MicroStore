using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Abstractions.Common;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.Abstractions.Queries;
using MicroStore.Ordering.Application.Abstractions.StateMachines;
using System.Net;
namespace MicroStore.Ordering.Application.Queries
{
    public class GetOrderQueryHandler : QueryHandler<GetOrderQuery>
    {

        private readonly IOrderDbContext _orderDbContext;

        public GetOrderQueryHandler(IOrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public override async Task<ResponseResult> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var query = _orderDbContext
                .Query<OrderStateEntity>()
                .AsNoTracking()
                .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);


            var result = await query.SingleOrDefaultAsync(x => x.OrderId == request.OrderId, cancellationToken);

            if(result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"order with id :{request.OrderId} is not exist");
            }

            return Success(HttpStatusCode.OK,result);
                
        }
    }
}
