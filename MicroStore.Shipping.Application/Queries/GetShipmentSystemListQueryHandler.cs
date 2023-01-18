using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.Application.Queries
{
    public class GetShipmentSystemListQueryHandler : QueryHandler<GetShipmentSystemListQuery,ListResultDto<ShipmentSystemDto>>
    {
        private readonly IShippingDbContext _shippingDbContext;

        public GetShipmentSystemListQueryHandler( IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public override async Task<ResponseResult<ListResultDto<ShipmentSystemDto>>> Handle(GetShipmentSystemListQuery request, CancellationToken cancellationToken)
        {
            var query = _shippingDbContext.ShippingSystems
                .AsNoTracking()
                .ProjectTo<ShipmentSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.ToListAsync(cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }
    }
}
