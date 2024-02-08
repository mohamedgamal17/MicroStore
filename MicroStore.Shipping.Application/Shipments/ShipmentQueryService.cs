using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Extensions;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;
using static MassTransit.ValidationResultExtensions;
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

        public async Task<Result<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
               .AsNoTracking()
               .Include(x => x.Items);

            var result = await query.SingleOrDefaultAsync(x => x.Id == shipmentId, cancellationToken);

            if (result == null)
            {
                return new Result<ShipmentDto>(new EntityNotFoundException(typeof(Shipment), shipmentId));
            }

            return ObjectMapper.Map<Shipment,ShipmentDto>(result);
        }

        public async Task<Result<ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
               .AsNoTracking();

            var result = await query.SingleOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

            if (result == null)
            {
                return new Result<ShipmentDto>(new EntityNotFoundException($"shipment with order id {orderId} is not exist"));
            }

            return ObjectMapper.Map<Shipment, ShipmentDto>(result);
        }

        public async Task<Result<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {

            var query = _shippingDbContext.Shipments
               .AsNoTracking();

            var result = await query.SingleOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

            if (result == null)
            {
                return new Result<ShipmentDto>(new EntityNotFoundException($"shipment with order number {orderNumber} is not exist"));
            }

            return ObjectMapper.Map<Shipment, ShipmentDto>(result);
        }

        public async Task<Result<PagedResult<ShipmentDto>>> ListAsync(ShipmentListQueryModel queryParams, string? userId = null ,CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.Shipments
                 .AsNoTracking()
                 .AsQueryable();

            query =  ApplyQueryFilter(query, queryParams, userId);

            var result = await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

            return ObjectMapper.Map<PagedResult<Shipment>, PagedResult<ShipmentDto>>(result);
        }

        public async Task<Result<List<ShipmentDto>>> ListByOrderIds(List<string> orderIds, CancellationToken cancellationToken = default)
        {
            if(orderIds != null && orderIds.Count > 0)
            {
                var query = _shippingDbContext.Shipments.AsNoTracking()
                    .AsQueryable();

                var shipments = await query.Where(x => orderIds.Contains(x.OrderId)).ToListAsync();


                return ObjectMapper.Map<List<Shipment>, List<ShipmentDto>>(shipments);
            }

            return new List<ShipmentDto>();
        }

        public async Task<Result<List<ShipmentDto>>> ListByOrderNumbers(List<string> orderNumbers, CancellationToken cancellationToken = default)
        {
            if (orderNumbers != null && orderNumbers.Count > 0)
            {
                var query = _shippingDbContext.Shipments.AsNoTracking()
                    .AsQueryable();

                var shipments = await query.Where(x => orderNumbers.Contains(x.OrderNumber)).ToListAsync();


                return ObjectMapper.Map<List<Shipment>, List<ShipmentDto>>(shipments);
            }

            return new List<ShipmentDto>();
        }


        public async Task<Result<PagedResult<ShipmentDto>>> SearchByOrderNumber(ShipmentSearchByOrderNumberModel model, CancellationToken cancellationToken = default)
        {
            var shipmentsQuery = _shippingDbContext.Shipments
                .AsNoTracking()
                .AsQueryable();

            shipmentsQuery = from shipment in shipmentsQuery
                             where shipment.OrderNumber == model.OrderNumber
                                || shipment.OrderNumber.StartsWith(model.OrderNumber)
                                || shipment.OrderNumber.Contains(model.OrderNumber)
                            select shipment;

            var result= await shipmentsQuery.PageResult(model.Skip, model.Length, cancellationToken);

            return ObjectMapper.Map<PagedResult<Shipment>, PagedResult<ShipmentDto>>(result);
        }

        private IQueryable<Shipment> ApplyQueryFilter(IQueryable<Shipment> query , ShipmentListQueryModel model ,string?userId = null )
        {
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(model.OrderNumber))
            {
                var orderNumber = model.OrderNumber.ToLower();
                query = query.Where(x => x.OrderNumber.ToLower().Contains(orderNumber));
            }

            if (!string.IsNullOrEmpty(model.TrackingNumber))
            {
                var tracknumber = model.TrackingNumber.ToLower();

                query = query.Where(x => x.TrackingNumber.ToLower().Contains(tracknumber));
            }

            if (model.Status != -1)
            {
                var state = (ShipmentStatus)model.Status;

                query = query.Where(x => x.Status == state);
            }

            if (!string.IsNullOrEmpty(model.Country))
            {
                query = query.Where(x => x.Address.CountryCode == model.Country);
            }

            if(model.StartDate != DateTime.MinValue)
            {
                var startDate = model.StartDate;
                query = query.Where(x => x.CreationTime >= startDate);
            }

            if (model.EndDate != DateTime.MinValue)
            {
                var endDate = model.EndDate;
                query = query.Where(x => x.CreationTime <= endDate);
            }

            query = query.OrderByDescending(x => x.CreationTime);

            return query;

        }
    }
}
