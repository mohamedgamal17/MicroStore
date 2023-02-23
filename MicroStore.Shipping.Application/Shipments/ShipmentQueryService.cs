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

        public async Task<UnitResult<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
               .AsNoTracking()
               .ProjectTo<ShipmentDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.NotFound( $"shipment with id {shipmentId} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
               .AsNoTracking()
               .ProjectTo<ShipmentDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.NotFound($"shipment with order id {orderId} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {

            var query = _shippingDbContext.Shipments
               .AsNoTracking()
               .ProjectTo<ShipmentDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ShipmentDto>(ErrorInfo.NotFound($"shipment with order number {orderNumber} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<PagedResult<ShipmentListDto>>> ListAsync(PagingQueryParams queryParams,string? userId = null ,CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
                 .AsNoTracking()
                 .ProjectTo<ShipmentListDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if(userId != null)
            {
                query = query.Where(x=> x.Id == userId);
            }

            var result = await query.PageResult(queryParams.PageNumber, queryParams.PageSize, cancellationToken);

            return UnitResult.Success(result);
        }
    }
}
