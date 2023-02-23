using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Shipping.Application.ShippingSystems
{
    public class ShippingSystemQueryService : ShippingApplicationService, IShippingSystemQueryService
    {
        private readonly IShippingDbContext _shippingDbContext;

        public ShippingSystemQueryService(IShippingDbContext shippingDbContext)
        {
            _shippingDbContext = shippingDbContext;
        }

        public async Task<UnitResult<ShippingSystemDto>> GetAsync(string systemId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.ShippingSystems
               .AsNoTracking()
               .ProjectTo<ShippingSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == systemId, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ShippingSystemDto>(ErrorInfo.NotFound($"system with id {systemId} is not exist"));
            }

            return UnitResult.Success(result);
        }   

        public async Task<UnitResult<ShippingSystemDto>> GetByNameAsync(string systemName, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.ShippingSystems
              .AsNoTracking()
              .ProjectTo<ShippingSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Name == systemName, cancellationToken);

            if (result == null)
            {
                return UnitResult.Failure<ShippingSystemDto>(ErrorInfo.NotFound($"system with name {systemName} is not exist"));
            }

            return UnitResult.Success(result);
        }

        public async Task<UnitResult<List<ShippingSystemDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.ShippingSystems
                 .AsNoTracking()
                 .ProjectTo<ShippingSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.ToListAsync();

            return UnitResult.Success(result);
        }
    }
}
