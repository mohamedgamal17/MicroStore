using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Entities;
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

        public async Task<ResultV2<ShippingSystemDto>> GetAsync(string systemId, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.ShippingSystems
               .AsNoTracking()
               .ProjectTo<ShippingSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Id == systemId, cancellationToken);

            if (result == null)
            {
                return new ResultV2<ShippingSystemDto>(new EntityNotFoundException(typeof(ShippingSystem), systemId));
            }

            return result;
        }   

        public async Task<ResultV2<ShippingSystemDto>> GetByNameAsync(string systemName, CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.ShippingSystems
              .AsNoTracking()
              .ProjectTo<ShippingSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.SingleOrDefaultAsync(x => x.Name == systemName, cancellationToken);

            if (result == null)
            {
                return new ResultV2<ShippingSystemDto>(new EntityNotFoundException($"system with name {systemName} is not exist"));
            }

            return result;
        }

        public async Task<ResultV2<List<ShippingSystemDto>>> ListAsync(CancellationToken cancellationToken = default)
        {
            var query = _shippingDbContext.ShippingSystems
                 .AsNoTracking()
                 .ProjectTo<ShippingSystemDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.ToListAsync();

            return result;
        }
    }
}
