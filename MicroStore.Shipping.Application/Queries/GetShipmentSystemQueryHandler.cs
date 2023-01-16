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
    public class GetShipmentSystemQueryHandler : QueryHandler<GetShipmentSystemQuery>
    {
        private readonly IShippingDbContext _shippingDbContext;

        public GetShipmentSystemQueryHandler( IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public override async Task<ResponseResult> Handle(GetShipmentSystemQuery request, CancellationToken cancellationToken)
        {
            var query = _shippingDbContext.ShippingSystems
                .AsNoTracking()
                .ProjectTo<ShipmentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == request.SystemId, cancellationToken);

            if(result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"system with id {request.SystemId} is not exist");
            }

            return Success(HttpStatusCode.OK, result);
        }
    }
}
