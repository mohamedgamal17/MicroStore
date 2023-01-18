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
    public class GetUserShipmentListQueryHandler : QueryHandler<GetUserShipmentListQuery,PagedResult<ShipmentListDto>>
    {
        private readonly IShippingDbContext _shippingDbContext;

        public GetUserShipmentListQueryHandler( IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public override async Task<ResponseResult<PagedResult<ShipmentListDto>>> Handle(GetUserShipmentListQuery request, CancellationToken cancellationToken)
        {
            var query = _shippingDbContext.Shipments
                  .AsNoTracking()
                  .ProjectTo<ShipmentListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query
                .Where(x=> x.UserId == request.UserId)
                .PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }
    }
}
