using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;
namespace MicroStore.Shipping.Application.Queries
{
    public class GetShipmentWithOrderIdQueryHandler : QueryHandler<GetShipmentWithOrderIdQuery,ShipmentDto>
    {
        private readonly IShippingDbContext _shippingDbContext;

        public GetShipmentWithOrderIdQueryHandler( IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public override async Task<ResponseResult<ShipmentDto>> Handle(GetShipmentWithOrderIdQuery request, CancellationToken cancellationToken)
        {
            var query = _shippingDbContext.Shipments
                .AsNoTracking()
                .ProjectTo<ShipmentDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderId == request.OrderId,cancellationToken);

            if(result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"shipment with order id {request.OrderId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
