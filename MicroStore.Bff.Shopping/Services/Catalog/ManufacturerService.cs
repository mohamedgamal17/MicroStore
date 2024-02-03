using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Grpc.Catalog;
using MicroStore.Bff.Shopping.Models.Catalog.Manufacturers;

namespace MicroStore.Bff.Shopping.Services.Catalog
{
    public class ManufacturerService
    {
        private readonly Grpc.Catalog.ManufacturerService.ManufacturerServiceClient _manufacturerServiceClient;

        public ManufacturerService(Grpc.Catalog.ManufacturerService.ManufacturerServiceClient manufacturerServiceClient)
        {
            _manufacturerServiceClient = manufacturerServiceClient;
        }

        public async Task<List<Manufacturer>> ListAsync(string? name = null, string? sortBy = null, bool desc = false)
        {
            var request = new ManufacturerListRequest
            {
                Name = name,
                SortBy = sortBy,
                Desc = desc
            };

            var response = await _manufacturerServiceClient.GetListAsync(request);

            var result = response.Data.Select(PrepareManufacturer).ToList();

            return result;
        }

        public async Task<Manufacturer> GetAsync(string manufacturerId  , CancellationToken cancellationToken = default)
        {
            var request = new GetManufacturerByIdRequest { Id = manufacturerId };

            var response = await _manufacturerServiceClient.GetByIdAsync(request);

            return PrepareManufacturer(response);
        }
        public async Task<Manufacturer> CreateAsync(ManufacturerModel model , CancellationToken cancellationToken = default)
        {
            var request = new CreateManufacturerRequest
            {
                Name = model.Name,
                Description = model.Description
            };

            var response = await _manufacturerServiceClient.CreateAsync(request);

            return PrepareManufacturer(response);
        }

        public async Task<Manufacturer> UpdateAsync(string manufacturerId ,ManufacturerModel model)
        {
            var request = new UpdateManufacturerRequest
            {
                Id = manufacturerId,
                Name = model.Name,
                Description = model.Description
            };

            var response = await _manufacturerServiceClient.UpdateAsync(request);

            return PrepareManufacturer(response);
        }


        private Manufacturer PrepareManufacturer (ManufacturerResponse response)
        {
            return new Manufacturer
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                CreatedAt = response.CreatedAt.ToDateTime(),
                ModifiedAt = response.ModifiedAt?.ToDateTime()
            };
        }
    }
}
