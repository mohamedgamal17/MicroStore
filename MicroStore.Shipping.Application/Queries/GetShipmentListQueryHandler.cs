using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;

namespace MicroStore.Shipping.Application.Queries
{
    public class GetShipmentListQueryHandler : QueryHandler<GetShipmentListQuery,PagedResult<ShipmentListDto>>
    {
        private readonly IShippingDbContext _shippingDbContext;

        public GetShipmentListQueryHandler( IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public override async Task<ResponseResult<PagedResult<ShipmentListDto>>> Handle(GetShipmentListQuery request, CancellationToken cancellationToken)
        {
            var query = _shippingDbContext.Shipments
                .AsNoTracking()
                .ProjectTo<ShipmentListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(request.PageNumber,request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }
    }
}
