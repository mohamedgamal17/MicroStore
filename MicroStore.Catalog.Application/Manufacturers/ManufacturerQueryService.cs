using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Manufacturers;
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

        public async Task<Result<List<ManufacturerDto>>> ListAsync(ManufacturerListQueryModel queryParams, CancellationToken cancellationToken = default)
        {
            var query =  _catalogDbContext.Manufacturers.AsNoTracking()
                .ProjectTo<ManufacturerDto>(MapperAccessor.Mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                var name = queryParams.Name.ToLower();

                query = query.Where(x => x.Name.ToLower().Contains(name));
            }

            if (queryParams.SortBy != null)
            {
                query = TryToSort(query, queryParams.SortBy, queryParams.Desc);
            }

            return await query.ToListAsync(cancellationToken);
        }
        private IQueryable<ManufacturerDto> TryToSort(IQueryable<ManufacturerDto> query, string sortBy , bool desc)
        {
            return sortBy.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                _ => query
            };
        }

    }
}
