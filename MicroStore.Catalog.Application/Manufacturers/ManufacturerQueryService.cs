using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Manufacturers
{
    public class ManufacturerQueryService : CatalogApplicationService, IManufacturerQueryService
    {

        private readonly ICatalogDbContext _catalogDbContext;
        public ManufacturerQueryService( ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public async Task<Result<ManufacturerDto>> GetAsync(string manufacturerId, CancellationToken cancellationToken = default)
        {
            var manufacturer = await _catalogDbContext.Manufacturers.SingleOrDefaultAsync(x => x.Id == manufacturerId, cancellationToken);

            if(manufacturer == null)
            {
                return new Result<ManufacturerDto>(new EntityNotFoundException(typeof(Manufacturer), manufacturerId));
            }

            return ObjectMapper.Map<Manufacturer, ManufacturerDto>(manufacturer);
        }

        public async Task<Result<List<ManufacturerDto>>> ListAsync(SortingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query =  _catalogDbContext.Manufacturers.AsNoTracking()
                .ProjectTo<ManufacturerDto>(MapperAccessor.Mapper.ConfigurationProvider);


            return await query.ToListAsync(cancellationToken);
        }
   
    }
}
