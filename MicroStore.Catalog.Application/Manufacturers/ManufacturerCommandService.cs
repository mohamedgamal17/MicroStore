using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Manufacturers;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Manufacturers
{
    public class ManufacturerCommandService : CatalogApplicationService ,IManufacturerCommandService
    {
        private readonly IRepository<Manufacturer> _manufacturerRepository;

        public ManufacturerCommandService(IRepository<Manufacturer> manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<Result<ManufacturerDto>> CreateAsync(ManufacturerModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateManufacturer(model);

            if (validationResult.IsFailure)
            {
                return new Result<ManufacturerDto>(validationResult.Exception);
            }
            var manufacturer = new Manufacturer();

            PrepareManufacturerEntity(manufacturer, model);

            await _manufacturerRepository.InsertAsync(manufacturer, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Manufacturer, ManufacturerDto>(manufacturer);
        }

        public async Task<Result<ManufacturerDto>> UpdateAsync(string manufacturerId ,ManufacturerModel model, CancellationToken cancellationToken=  default)
        {
            var validationResult = await ValidateManufacturer(model, manufacturerId);

            if (validationResult.IsFailure)
            {
                return new Result<ManufacturerDto>(validationResult.Exception);
            }

            var manufacturer = await _manufacturerRepository.SingleOrDefaultAsync(x => x.Id == manufacturerId);

            if(manufacturer == null)
            {
                return new Result<ManufacturerDto>(new EntityNotFoundException(typeof(Manufacturer), manufacturerId));
            }

            PrepareManufacturerEntity(manufacturer, model);

            await _manufacturerRepository.UpdateAsync(manufacturer, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Manufacturer, ManufacturerDto>(manufacturer);
        }


        private async Task<Result<Unit>> ValidateManufacturer(ManufacturerModel model , string? manufacturerId = null, CancellationToken cancellationToken = default)
        {

            var query = await _manufacturerRepository.GetQueryableAsync();

            if(manufacturerId != null)
            {
                query = query.Where(x => x.Id != manufacturerId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name, cancellationToken))
            {
                return new Result<Unit>(new UserFriendlyException("Manufacturer name is already exist"));
            }

            return new Result<Unit>(Unit.Value);
        }

        private void PrepareManufacturerEntity(Manufacturer manufacturer , ManufacturerModel model)
        {
            manufacturer.Name = model.Name;
            manufacturer.Description = model.Description;
         }
    }
}
