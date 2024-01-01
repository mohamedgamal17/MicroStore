using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Geographic.Application.Countries
{
    public interface ICountryApplicationService : IApplicationService
    {
        Task<Result<CountryDto>> CreateAsync(CountryModel model, CancellationToken cancellationToken = default);

        Task<Result<CountryDto>> UpdateAsync(string countryId, CountryModel country , CancellationToken cancellationToken = default);

        Task<Result<Unit>> DeleteAsync(string countryId, CancellationToken cancellationToken = default);

        Task<List<CountryListDto>> ListAsync(CancellationToken cancellationToken = default);
        
        Task<Result<CountryDto>> GetAsync(string countryId, CancellationToken cancellationToken = default);

        Task<Result<CountryDto>> GetByCodeAsync(string countryCode,  CancellationToken cancellationToken = default);
    }
}
