using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace MicroStore.Shipping.Application.Shipments
{
    [DisableValidation]
    public class ShipmentQueryService : ShippingApplicationService, IShipmentQueryService
    {
        private readonly IShippingDbContext _shippingDbContext;

        public ShipmentQueryService(IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public async Task<UnitResultV2<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
               .AsNoTracking()
               .ProjectTo<ShipmentDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);

            if (result == null)
            {
                return UnitResultV2.Failure<ShipmentDto>(ErrorInfo.NotFound( $"shipment with id {shipmentId} is not exist"));
            }

            return UnitResultV2.Success(result);
        }

        public async Task<UnitResultV2<ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
               .AsNoTracking()
               .ProjectTo<ShipmentDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

            if (result == null)
            {
                return UnitResultV2.Failure<ShipmentDto>(ErrorInfo.NotFound($"shipment with order id {orderId} is not exist"));
            }

            return UnitResultV2.Success(result);
        }

        public async Task<UnitResultV2<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {

            var query = _shippingDbContext.Shipments
               .AsNoTracking()
               .ProjectTo<ShipmentDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return UnitResultV2.Failure<ShipmentDto>(ErrorInfo.NotFound($"shipment with order number {orderNumber} is not exist"));
            }

            return UnitResultV2.Success(result);
        }

        public async Task<UnitResultV2<PagedResult<ShipmentListDto>>> ListAsync(PagingQueryParams queryParams,string? userId = null ,CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
                 .AsNoTracking()
                 .ProjectTo<ShipmentListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(userId != null)
            {
                query = query.Where(x=> x.Id == userId);
            }

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResultV2.Success(result);
        }
    }
}
