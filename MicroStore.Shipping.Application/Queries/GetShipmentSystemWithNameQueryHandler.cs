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
    public class GetShipmentSystemWithNameQueryHandler : QueryHandler<GetShipmentSystemWithNameQuery,ShipmentSystemDto>
    {
        private readonly IShippingDbContext _shippingDbContext;

        public GetShipmentSystemWithNameQueryHandler(IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public override async Task<ResponseResult<ShipmentSystemDto>> Handle(GetShipmentSystemWithNameQuery request, CancellationToken cancellationToken)
        {
            var query = _shippingDbContext.ShippingSystems
                .AsNoTracking()
                .ProjectTo<ShipmentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x=> x.Name == request.Name,cancellationToken);

            if(result == null)
            {
                return Failure(HttpStatusCode.NotFound, $"shipment system with name {request.Name} is not exist");
            }

            return Success(HttpStatusCode.OK, result);

        }
    }
}
